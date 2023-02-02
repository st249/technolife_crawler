using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Infrastructure.DataAccess.Repositories;
using TechnolifeCrawler.Infrastructure.Utilities;
using TechnoligeCrawler.Abstractions.Utilities;
using TechnoligeCrawler.Infrastructure.Utilities;

namespace TechnolifeCrawler.StartupExtentions
{
    public static class ServicesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILaptopDetailPageParser, LaptopDetailPageParser>();
            services.AddScoped<IProductListPageParser, ProductListPageParser>();
            services.AddScoped<IHtmlManager, HtmlManager>();
            services.AddSingleton<IOperationLockManager, OperationLockManager>();


            //DATA ACCESS
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
