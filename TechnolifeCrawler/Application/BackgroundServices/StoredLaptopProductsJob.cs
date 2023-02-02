using HtmlAgilityPack;
using MassTransit;
using Microsoft.Extensions.Options;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Application.BackgroundServices;
using TechnolifeCrawler.Application.Events;
using TechnolifeCrawler.Infrastructure.Configurations;
using TechnolifeCrawler.Models.Dtos;
using TechnolifeCrawler.Models.Factories;
using TechnoligeCrawler.Abstractions.BackgroundServices;
using TechnoligeCrawler.Abstractions.Utilities;

namespace TechnoligeCrawler.Application.BackgroundServices;
public class StoredLaptopProductsJob : StoreProductsJobs
{
    private readonly StoreLaptopProductJobConfigurations _conf;
    public StoredLaptopProductsJob(Serilog.ILogger logger, IServiceProvider services, IOptions<StoreLaptopProductJobConfigurations> conf) :
        base(logger, services)
    {
        _conf = conf.Value;
    }


    protected async override Task<HtmlDocument> LoadDocumentWithPageNumber(int page)
    {
        using (var scoped = _services.CreateScope())
        {
            IHtmlManager htmlManager = scoped.ServiceProvider.GetRequiredService<IHtmlManager>();
            var requestUrl = $"{_conf.BaseUrl}/{_conf.ProductListUrl}&page={page}";
            return await htmlManager.DownloadHtmlDocumentFromUrl(requestUrl);
        }

    }

    protected override string GetCronExpression()
    {
        return _conf.CronExpression;
    }

    protected override async Task CreateNewProduct(ProductListItemDto productItem)
    {
        var foundedNewProductEvent = new NewLaptopFoundedEvent()
        {
            ProductItem = productItem
        };
        using (var scoped = _services.CreateScope())
        {
            IPublishEndpoint publisher = scoped.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publisher.Publish(foundedNewProductEvent);
        }
    }
}