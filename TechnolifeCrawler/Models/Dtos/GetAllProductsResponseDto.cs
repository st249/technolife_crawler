namespace TechnolifeCrawler.Models.Dtos
{
    public class GetAllProductsResponseDto
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public GetMenuProducts get_menu_products { get; set; }
    }

    public class GetMenuProducts
    {
        public List<TechnolifeSmallProduct> results { get; set; }
        public int count { get; set; }
    }

    public class Icon
    {
        public string font { get; set; }
        public string value { get; set; }
    }

    public class TechnolifeSmallProduct
    {
        public string name { get; set; }
        public string _id { get; set; }
        public string code { get; set; }
        public int Id => Convert.ToInt32(code.Split('-')[1]);
        public int normal_price { get; set; }
        public string discount { get; set; }
        public int? discounted_price { get; set; }
        public List<Icon> icons { get; set; }
        public int LaptopSize => Convert.ToInt32(icons.FirstOrDefault(e => e.font == "icon-monitor")?.value ?? "0");
        public string deadline { get; set; }
        public List<string> colors { get; set; }
        public string image { get; set; }
        public string alt_image { get; set; }
        public int score_count { get; set; }
        public int score_avg { get; set; }
        public int available { get; set; }
        public string marketing_group { get; set; }
        public object warningCount { get; set; }
        public bool show_color { get; set; }
    }


}
