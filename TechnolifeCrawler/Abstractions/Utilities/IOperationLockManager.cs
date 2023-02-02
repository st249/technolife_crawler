namespace TechnoligeCrawler.Abstractions.Utilities;
public interface IOperationLockManager
{
    Task<bool> IsLockedAsync(string key, TimeSpan keyTimeOut);
    Task<bool> ReleaseLockAsync(string lockKey);
}