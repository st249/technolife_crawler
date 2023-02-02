namespace TechnolifeCrawler.Infrastructure.Configurations;

public class RabbitMqConfigurations
{
    public const string Key = "RabbitMq";
    public string Host { get; set; }
    public string VirtualHost { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
