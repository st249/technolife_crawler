using TechnoligeCrawler.Application.BackgroundServices;

namespace TechnolifeCrawler.StartupExtentions;

public static class HostedServicesInjection
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<StoredLaptopProductsJob>();

        return services;
    }
}
