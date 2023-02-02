using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;
using TechnolifeCrawler.Abstractions.DataAccess.Repositories;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Infrastructure.Configurations;
using TechnolifeCrawler.Models.Dtos;
using TechnolifeCrawler.Models.Factories;
using TechnoligeCrawler.Abstractions.Utilities;

namespace TechnolifeCrawler.Application.Events;

public class NewLaptopFoundedEvent
{
    public ProductListItemDto ProductItem { get; set; }
}

public class NewLaptopFoundedEventHandler : IConsumer<NewLaptopFoundedEvent>
{
    private readonly StoreLaptopProductJobConfigurations _conf;
    private readonly ILaptopDetailPageParser _laptopDetailPageParser;
    private readonly IHtmlManager _htmlManager;
    private readonly IOperationLockManager _operationLockManager;
    private readonly Serilog.ILogger _logger;
    private readonly IProductRepository _productRepo;

    public NewLaptopFoundedEventHandler(IOptions<StoreLaptopProductJobConfigurations> conf, ILaptopDetailPageParser laptopDetailPageParser, IHtmlManager htmlManager, IOperationLockManager operationLockManager, Serilog.ILogger logger, IProductRepository productRepo)
    {
        _conf = conf.Value;
        _laptopDetailPageParser = laptopDetailPageParser;
        _htmlManager = htmlManager;
        _operationLockManager = operationLockManager;
        _logger = logger;
        _productRepo = productRepo;
    }


    public async Task Consume(ConsumeContext<NewLaptopFoundedEvent> context)
    {

        var productItem = context.Message.ProductItem;
        var lockKey = nameof(NewLaptopFoundedEvent) + "-" + productItem.Id;
        if (await _operationLockManager.IsLockedAsync(lockKey, TimeSpan.FromMinutes(5)))
        {
            _logger.Information($"Another consumer is processing this item: {productItem.Id}");
            return;
        }
        var requestUrl = $"{_conf.BaseUrl}/product-{productItem.Id}/product";
        var htmlDoc = await _htmlManager.DownloadHtmlDocumentFromUrl(requestUrl);
        var laptopDetailDto = await _laptopDetailPageParser.ParseAsync(htmlDoc);
        var newLaptop = ProductFactory.CreateLaptop(productItem.Id, productItem.Title, productItem.ImageAddress, 0, "", null);
        await _productRepo.InserNewProductAsync(newLaptop);
        await _operationLockManager.ReleaseLockAsync(lockKey);
    }
}
