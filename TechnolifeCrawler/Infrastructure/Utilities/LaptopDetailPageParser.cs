using HtmlAgilityPack;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Drawing;
using TechnolifeCrawler.Abstractions.Utilities;
using TechnolifeCrawler.Infrastructure.Configurations;
using TechnolifeCrawler.Models.Dtos;

namespace TechnolifeCrawler.Infrastructure.Utilities
{
    public class LaptopDetailPageParser : ILaptopDetailPageParser
    {
        private readonly CrawlerConfigurations _conf;

        public LaptopDetailPageParser(IOptions<CrawlerConfigurations> conf)
        {
            _conf = conf.Value;
        }

        public LaptopDetailDto Parse(HtmlDocument doc)
        {
            var result = new LaptopDetailDto();

            var featuresContainer = doc.DocumentNode.Descendants().Where(e => e.Id == _conf.DetailContainerDivId);
            return null;
        }
    }
}
