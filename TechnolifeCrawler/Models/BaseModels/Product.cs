using TechnolifeCrawler.Models.BaseModels;
using TechnolifeCrawler.Models.Enums;

namespace TechnoligeCrawler.Models.BaseModels;
public class Product
{
    private ICollection<ProductImage> _images;
    public Guid Id { get; private set; }
    public DateTime CreationTime { get; private set; }
    public int TechnolifeId { get; private set; }
    public string Title { get; private set; }
    public string ImageAddress { get; private set; }
    public decimal Price { get; private set; }
    public string Brand { get; set; }
    public ICollection<ProductFeature> ProductFeatures { get; private set; }
    public IReadOnlyCollection<ProductImage> Images => _images?.ToList() ?? new List<ProductImage>();
    public ProductCategory Category { get; private set; }

    public Product(int technolifeId, string title, string imageAddress, decimal price, string brand, ICollection<ProductFeature> productFeatures, ProductCategory category)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
        TechnolifeId = technolifeId;
        Title = title;
        ImageAddress = imageAddress;
        Price = price;
        Brand = brand;
        ProductFeatures = productFeatures;
        Category = category;
        _images = new List<ProductImage>();
    }

    public void AddImage(string imageAddress)
    {
        _images.Add(new ProductImage(imageAddress));
    }
    private Product() { }
}