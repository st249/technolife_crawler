using TechnolifeCrawler.Models.Dtos;

namespace TechnolifeCrawler.Abstractions.Utilities
{
    public interface IProductListPageParser : IHtmlDocumentParser<List<ProductListItemDto>>
    {
    }
}
