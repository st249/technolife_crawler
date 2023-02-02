namespace TechnolifeCrawler.Infrastructure.Configurations
{
    public class RedisConfigurations
    {
        public const string Key = "Redis";
        public string ConnectionString { get; set; }
        public int DatabaseId { get; set; }
    }
}
