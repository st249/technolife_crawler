using TechnolifeCrawler.Models.Enums;

namespace TechnoligeCrawler.Models.BaseModels;
public class ProductFeature
{
    public Guid Id { get; set; }    
    public Product Product { get; set; }
    public FeatureKey Key { get; set; }
    public string Value { get; set; }

}