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
    private readonly ILaptopDetailPageParser _laptopDetailPageParser;
    private readonly IHtmlManager _htmlManager;
    private readonly IPublishEndpoint _publisher;
    public StoredLaptopProductsJob(Serilog.ILogger logger, IServiceProvider services, IOptions<StoreLaptopProductJobConfigurations> conf, IProductListPageParser productListPageParser, ILaptopDetailPageParser laptopDetailPageParser,
        IHtmlManager htmlManager, IProductRepository productRepo, IPublishEndpoint publisher) :
        base(logger, services, productListPageParser, productRepo)
    {
        _conf = conf.Value;
        _laptopDetailPageParser = laptopDetailPageParser;
        _htmlManager = htmlManager;
        _publisher = publisher;
    }


    protected async override Task<HtmlDocument> LoadDocumentWithPageNumber(int page)
    {
        var requestUrl = $"{_conf.BaseUrl}/{_conf.ProductListUrl}&page={page}";
        return await _htmlManager.DownloadHtmlDocumentFromUrl(requestUrl);
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
        await _publisher.Publish(foundedNewProductEvent);
    }
}