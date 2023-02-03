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

            var featuresContainer = doc.DocumentNode.Descendants().Where(e => e.Id == _conf.DetailContainerDivId).First();

            var features = featuresContainer.ChildNodes.Where(e => e.Name == "li");

            GetWeight(result, features);

        }

        private static void GetWeight(LaptopDetailDto result, IEnumerable<HtmlNode> features)
        {
            foreach (var feature in features)
            {
                var featureTitle = feature.ChildNodes.Where(e => e.Name == "div")
                    .First().InnerText;
                if (featureTitle == "وزن")
                {
                    var weight = feature.ChildNodes.Where(e => e.Name == "div")
                     .Last().InnerText;
                    result.ProductFeatures.Add(new TechnoligeCrawler.Models.BaseModels.ProductFeature()
                    {
                        Id = Guid.NewGuid(),
                        Key = Models.Enums.FeatureKey.Laptop_Weight,
                        Value = weight
                    });
                    break;
                }
            }
        }
    }
}
