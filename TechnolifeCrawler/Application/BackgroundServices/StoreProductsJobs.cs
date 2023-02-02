using HtmlAgilityPack;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Models.Dtos;
using TechnoligeCrawler.Abstractions.BackgroundServices;

namespace TechnolifeCrawler.Application.BackgroundServices
{
    public abstract class StoreProductsJobs : ScheduledBackgroundService
    {
        private readonly IProductListPageParser _pageParser;
        private readonly IProductRepository _productRepo;
        protected StoreProductsJobs(Serilog.ILogger logger, IServiceProvider services, IProductListPageParser pageParser, IProductRepository productRepo) : base(logger, services)
        {
            _pageParser = pageParser;
            _productRepo = productRepo;
        }

        protected async override Task ExecuteAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
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
            var products = await _pageParser.ParseAsync(doc);
            var result = new List<ProductListItemDto>();
            foreach (var product in products)
            {
                if (!await _productRepo.IsProductExistsAsync(product.Id))
                    result.Add(product);
            }
            return result;
        }
    }
}
