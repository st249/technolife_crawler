using HtmlAgilityPack;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Models.Dtos;
using TechnoligeCrawler.Abstractions.BackgroundServices;

namespace TechnolifeCrawler.Application.BackgroundServices
{
    public abstract class StoreProductsJobs : ScheduledBackgroundService
    {
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
                HtmlDocument doc = await LoadDocumentWithPageNumber(page);

                List<ProductListItemDto> newProducts = await GetNewProducts(doc);

                if (!newProducts.Any())
                {
                    hasNewData = false;
                    break;
                }

                foreach (var productItem in newProducts)
                {
                    await CreateNewProduct(productItem);
                }

                page++;

            } while (hasNewData == true);
        }

        protected abstract Task<HtmlDocument> LoadDocumentWithPageNumber(int page);

        protected abstract Task CreateNewProduct(ProductListItemDto productItem);

        protected async virtual Task<List<ProductListItemDto>> GetNewProducts(HtmlDocument doc)
        {
            using (var scope = _services.CreateScope())
            {
                IProductListPageParser scopedProductListPageParser =
                scope.ServiceProvider.GetRequiredService<IProductListPageParser>();

                IProductRepository scopedProductRepo =
                    scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var products = scopedProductListPageParser.Parse(doc);
                var result = new List<ProductListItemDto>();
                foreach (var product in products)
                {
                    if (!await scopedProductRepo.IsProductExistsAsync(product.Id))
                        result.Add(product);
                }
                return result;

            }

        }
    }
}
