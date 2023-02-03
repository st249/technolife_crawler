using HtmlAgilityPack;
using System.Drawing;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Models.Dtos;
using TechnoligeCrawler.Abstractions.BackgroundServices;

namespace TechnolifeCrawler.Application.BackgroundServices
{
    public abstract class StoreProductsJobs : ScheduledBackgroundService
    {
        protected const int _pageSize = 32;
        protected StoreProductsJobs(Serilog.ILogger logger, IServiceProvider services) : base(logger, services)
        {

        }

        protected async override Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.Debug("Starting importing products from technolife");
            var hasNewData = true;
            var page = 1;
            do
            {

                List<TechnolifeSmallProduct> newProducts = await GetNewProducts(page);

                if (!newProducts.Any())
                {
                    hasNewData = false;
                }
                else
                {
                    foreach (var productItem in newProducts)
                    {
                        await CreateNewProduct(productItem);
                    }
                }
                page++;

            } while (hasNewData == true);
        }

        protected abstract Task CreateNewProduct(TechnolifeSmallProduct productItem);

        protected async virtual Task<List<TechnolifeSmallProduct>> GetNewProducts(int page)
        {
            var result = new List<TechnolifeSmallProduct>();
            var productList = await GetPagedListFromTechnolife(page);
            using (var scoped = _services.CreateScope())
            {
                var scopedProductRepo = scoped.ServiceProvider.GetRequiredService<IProductRepository>();
                foreach (var smallProduct in productList)
                {
                    if (!await scopedProductRepo.IsProductExistsAsync(smallProduct.Id))
                    {
                        result.Add(smallProduct);
                    }
                }
                return result;

            }
        }

        protected abstract Task<List<TechnolifeSmallProduct>> GetPagedListFromTechnolife(int page);

    }
}
