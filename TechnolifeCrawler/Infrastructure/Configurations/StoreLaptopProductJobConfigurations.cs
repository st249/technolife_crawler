namespace TechnolifeCrawler.Infrastructure.Configurations;

public class StoreLaptopProductJobConfigurations
{
    public const string Key = "StoreLaptopProductsJob";
    public string CronExpression { get; set; }
    public string BaseUrl { get; set; }
    public string ProductListUrl { get; set; }

}
