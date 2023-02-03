namespace TechnolifeCrawler.Abstractions.Utilities
{
    public interface IHttpRequest
    {
        Task<T> PostAsync<T>(string url, object model,
    Dictionary<string, string> headers = null);
    }
}
