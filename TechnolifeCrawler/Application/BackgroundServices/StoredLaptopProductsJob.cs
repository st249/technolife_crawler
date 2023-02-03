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

    protected override string GetCronExpression()
    {
        return _conf.CronExpression;
    }

    protected override async Task CreateNewProduct(TechnolifeSmallProduct productItem)
    {
        var foundedNewProductEvent = new NewLaptopFoundedEvent()
        {
            //ProductItem = productItem
        };
        using (var scoped = _services.CreateScope())
        {
            IPublishEndpoint publisher = scoped.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            await publisher.Publish(foundedNewProductEvent);
        }
    }

    protected override async Task<List<TechnolifeSmallProduct>> GetPagedListFromTechnolife(int page)
    {
        var getLaptopListInput = new GetAllProductsInput();
        getLaptopListInput.Query = " query get_menu_products($url: String, $filterObj: filter_obj){\n          get_menu_products(url: $url, filterObj: $filterObj){\n            results{\n              name\n              _id\n              code\n              normal_price\n              discount\n              discounted_price\n              icons{\n                font\n                value\n              }\n              code\n              deadline\n              colors\n              image\n              alt_image\n              score_count\n              score_avg\n              available\n              marketing_group\n              warningCount\n              show_color\n         }\n            count\n            banners{\n              url\n              link\n              alt\n              query\n            }\n          }\n        }";
        getLaptopListInput.Variables = new GetAllProductsVariables()
        {
            FilterObj = new GetAllProductsFilterObject()
            {
                Available = null,
                Limit = _pageSize,
                Ordering = "date-desc",
                Skip = (page - 1)
            },
            Url = _conf.LaptopListUrl
        };
        using (var scoped = _services.CreateScope())
        {
            var scopedHttpRequest = scoped.ServiceProvider.GetRequiredService<IHttpRequest>();
            var response = await scopedHttpRequest.PostAsync<GetAllProductsResponseDto>(_conf.BaseUrl, getLaptopListInput);

            return response.data.get_menu_products.results.Select(e => e).ToList();
        }
    }
}