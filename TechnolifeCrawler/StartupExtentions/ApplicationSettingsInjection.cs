using TechnolifeCrawler.Infrastructure.Configurations;

namespace TechnolifeCrawler.StartupExtentions
{
    public static class ApplicationSettingsInjection
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfigurations>(configuration.GetSection(RabbitMqConfigurations.Key));
            services.Configure<StoreLaptopProductJobConfigurations>(configuration.GetSection(StoreLaptopProductJobConfigurations.Key));
            services.Configure<RedisConfigurations>(configuration.GetSection(RedisConfigurations.Key));
            services.Configure<CrawlerConfigurations>(configuration.GetSection(CrawlerConfigurations.Key));

            return services;
        }
    }
}
