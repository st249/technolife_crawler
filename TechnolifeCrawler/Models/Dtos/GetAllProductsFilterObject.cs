namespace TechnolifeCrawler.Models.Dtos;

public class GetAllProductsFilterObject
{
    public object Available { get; set; }
    public int Skip { get; set; }
    public string Ordering { get; set; }
    public int Limit { get; set; }
}
