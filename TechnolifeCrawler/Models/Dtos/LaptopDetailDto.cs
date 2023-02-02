using TechnoligeCrawler.Models.BaseModels;

namespace TechnolifeCrawler.Models.Dtos
{
    public class LaptopDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageAddress { get; set; }
        public decimal price { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }

        public LaptopDetailDto(ProductListItemDto dto)
        {
            Id = dto.Id;
            Title = dto.Title;
            ImageAddress = dto.ImageAddress;
            price = dto.Price;
        }

        public LaptopDetailDto() { }
    }
}
