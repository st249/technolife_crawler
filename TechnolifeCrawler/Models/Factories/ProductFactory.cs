using TechnolifeCrawler.Models.Enums;
using TechnoligeCrawler.Models.BaseModels;

namespace TechnolifeCrawler.Models.Factories;

public static class ProductFactory
{
    public static Product CreateLaptop(int technolifeId, string title, string imageAddress, decimal price, string brand, ProductFeatures productFeatures)
    {
        return new Product(technolifeId, title, imageAddress, price, brand, productFeatures, ProductCategory.Laptop);
    }
}
