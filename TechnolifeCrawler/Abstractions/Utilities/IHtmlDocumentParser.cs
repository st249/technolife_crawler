using HtmlAgilityPack;

namespace TechnolifeCrawler.Abstractions.Utilities;

public interface IHtmlDocumentParser<TOutput>
{
    Task<TOutput> ParseAsync(HtmlDocument doc);
}
