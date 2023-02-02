using HtmlAgilityPack;

namespace TechnoligeCrawler.Abstractions.Utilities;
//TODO:Better Name
public interface IHtmlManager
{
    Task<HtmlDocument> DownloadHtmlDocumentFromUrl(string url);
}