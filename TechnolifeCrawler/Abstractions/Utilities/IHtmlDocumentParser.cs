using HtmlAgilityPack;

namespace TechnolifeCrawler.Abstractions.Utilities;

public interface IHtmlDocumentParser<TOutput>
{
    TOutput Parse(HtmlDocument doc);
}
