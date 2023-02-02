using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using TechnolifeCrawler.Infrastructure.Configurations;
using TechnoligeCrawler.Abstractions.Utilities;

namespace TechnolifeCrawler.Infrastructure.Utilities;

public class OperationLockManager : IOperationLockManager
{
    private IDatabase _database { get { return _connectionMultiplexer.GetDatabase(); } }
    private readonly RedisConfigurations _redisConf;
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public OperationLockManager(IOptions<RedisConfigurations> conf)
    {
        _redisConf = conf.Value;    
        _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisConf.ConnectionString);
    }

    public async Task<bool> IsLockedAsync(string key, TimeSpan? keyTimeOut = null)
    {

        var redisKey = new RedisKey(key);

        keyTimeOut ??= new TimeSpan(0, 1, 0);

        var value = await _database.StringGetSetAsync(redisKey, new RedisValue($"{key}-operation-is-locked"));

        if (value.HasValue) return true; //someone else has got the lock

        //We got the lock! set the expirce time
        await _database.KeyExpireAsync(redisKey, keyTimeOut);
        return false;
    }

    public async Task<bool> ReleaseLockAsync(string lockKey)
    {
        var redisKey = new RedisKey(lockKey);
        var result = await _database.KeyDeleteAsync(redisKey);
        return result;
    }
}
