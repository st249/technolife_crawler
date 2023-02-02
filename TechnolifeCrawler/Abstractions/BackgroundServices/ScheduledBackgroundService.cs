using NCrontab;
using TechnoligeCrawler.Abstractions.Utilities;

namespace TechnoligeCrawler.Abstractions.BackgroundServices;
public abstract class ScheduledBackgroundService : IHostedService, IDisposable
{
    protected readonly Serilog.ILogger _logger;
    protected readonly IOperationLockManager _lockManager;
    protected IServiceProvider _services;
    protected CrontabSchedule _cronSchedule;
    protected System.Timers.Timer? _timer;

    /// <summary>
    /// When Execution Timeout reaches, the OperationLock will be released automatically.
    /// </summary>
    /// <returns></returns>
    protected TimeSpan _executionTimeout = new TimeSpan(2, 0, 0); // 10 min

    public ScheduledBackgroundService(Serilog.ILogger logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
        _lockManager = _services.GetRequiredService<IOperationLockManager>();
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Information("Starting " + this.GetType().ToString());
        CreateCronSchedule();
        await ScheduleNextRun(cancellationToken);
    }

    private void CreateCronSchedule()
    {
        var cronExpression = GetCronExpression();
        _cronSchedule = CrontabSchedule.Parse(cronExpression,
            new CrontabSchedule.ParseOptions { IncludingSeconds = false });
    }

    protected Task<bool> IsLockedAsync()
    {
        var lockKey = this.GetType().Name;
        return _lockManager.IsLockedAsync(lockKey, _executionTimeout);
    }

    protected Task<bool> ReleaseLockAsync()
    {
        var lockKey = this.GetType().Name;
        return _lockManager.ReleaseLockAsync(lockKey);
    }


    protected async Task RunTask(CancellationToken cancellationToken)
    {
        if (!await IsLockedAsync())
        {
            using (var scope = _services.CreateScope())
            {
                try
                {
                    await ExecuteAsync(scope, cancellationToken);
                }
                catch (System.Exception e)
                {
                    _logger.Error(e, "Error On Running " + this.GetType().ToString());
                }
                finally
                {
                    await ReleaseLockAsync();
                }
            }
        }
        else
        {
            _logger.Debug("Couldn't Get Lock for Scheduled Background Service");
        }
    }

    protected async Task ScheduleNextRun(CancellationToken cancellationToken)
    {
        var nextRunTime = _cronSchedule.GetNextOccurrence(DateTime.UtcNow);
        var delay = nextRunTime - DateTimeOffset.UtcNow;
        if (delay.TotalMilliseconds <= 0) // prevent non-positive values from being passed into Timer
        {
            await ScheduleNextRun(cancellationToken);
        }

        _timer = new System.Timers.Timer(delay.TotalMilliseconds);
        _timer.Elapsed += async (sender, args) =>
        {
            _timer.Dispose(); // reset and dispose timer
            _timer = null;

            if (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await RunTask(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error On RunTask " + this.GetType().ToString());
                }
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                await ScheduleNextRun(cancellationToken); // reschedule next
            }
        };
        _timer.Start();
        await Task.CompletedTask;
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Stop();
        _logger.Information("Stopped " + this.GetType().ToString());
        await Task.CompletedTask;
    }

    protected abstract string GetCronExpression();


    protected abstract Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken);

    public void Dispose()
    {
        _timer?.Dispose();
    }
}