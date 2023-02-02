using HtmlAgilityPack;
using TechnoligeCrawler.Abstractions.Utilities;

namespace TechnoligeCrawler.Infrastructure.Utilities;
public class HtmlManager : IHtmlManager
{
    public async Task<HtmlDocument> DownloadHtmlDocumentFromUrl(string url)
    {

        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        return doc;
    }

}