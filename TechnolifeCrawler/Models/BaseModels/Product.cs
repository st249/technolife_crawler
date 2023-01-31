namespace TechnoligeCrawler.Models.BaseModels;
public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ImageAddress { get; set; }
    public decimal Price { get; set; }
    public ProductFeatures ProductFeatures { get; set; }
}