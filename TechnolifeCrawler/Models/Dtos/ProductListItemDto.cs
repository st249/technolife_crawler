namespace TechnolifeCrawler.Models.Dtos
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageAddress { get; set; }
        public string DetailsUrl { get; set; }
        public string StrPrice { get; set; }
        public decimal Price
        {
            get
            {
                if (string.IsNullOrEmpty(StrPrice))
                    return 0;
                var plainPrice = StrPrice.Replace("تومان","");
                plainPrice = plainPrice.Replace(",", "");
                return Convert.ToDecimal(plainPrice);
            }
        }
    }
}
