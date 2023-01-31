namespace TechnoligeCrawler.Abstractions.BackgroundServices;
public abstract class ScheduledBackgroundService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}