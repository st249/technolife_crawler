using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Infrastructure.Configurations;
using TechnolifeCrawler.Models.Dtos;

namespace TechnolifeCrawler.Infrastructure.Utilities;

public class ProductListPageParser : IProductListPageParser
{
    private readonly CrawlerConfigurations _conf;

    public ProductListPageParser(IOptions<CrawlerConfigurations> conf)
    {
        _conf = conf.Value;
    }

    public List<ProductListItemDto> Parse(HtmlDocument doc)
    {
        var result = new List<ProductListItemDto>();
        var container = doc.DocumentNode.Descendants().Where(e => e.Id == _conf.ListContainerDivId);
        if (container == null || container.FirstOrDefault() == null)
            return result;
        var ulElement = container.FirstOrDefault().ChildNodes.Where(e => e.Name == "ul").First();
        if (ulElement == null || ulElement.ChildNodes.Count == 0)
            return result;
        foreach (var liElement in ulElement.ChildNodes.Where(e => e.Name == "li"))
        {

            var id = liElement.Attributes["id"].Value;
            var title = liElement.ChildNodes.Where(e => e.Name == "a").First().Attributes["title"].Value;
            var href = liElement.ChildNodes.Where(e => e.Name == "a").First().Attributes["href"].Value;
            var imageAddress = liElement.ChildNodes.Where(e => e.Name == "a").First()
                .ChildNodes.Where(e => e.Name == "figure").First()
                .ChildNodes.Where(e => e.Name == "img").First().Attributes["src"].Value;
            var price = liElement.ChildNodes.Where(e => e.Name == "section").First()
                .ChildNodes.Where(e=>e.Name=="div").First()
                .ChildNodes.Where(e => e.Name == "span").First().InnerText;
            

            var newProduct = new ProductListItemDto()
            {
                Id = Int32.Parse(id),
                DetailsUrl = href,
                Title = title,
                ImageAddress = imageAddress,
                StrPrice = price

            };
            result.Add(newProduct);
        }
        return result;
    }
}
