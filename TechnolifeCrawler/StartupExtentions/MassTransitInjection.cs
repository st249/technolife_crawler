using MassTransit;
using TechnolifeCrawler.Application.Events;
using TechnolifeCrawler.Infrastructure.Configurations;

namespace TechnolifeCrawler.StartupExtentions
{
    public static class MassTransitInjection
    {
        public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services,
           IConfiguration configuration)
        {
            var rabbitOption = new RabbitMqConfigurations();
            configuration.GetSection(RabbitMqConfigurations.Key).Bind(rabbitOption);
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new DefaultEndpointNameFormatter("Crawler", false));
                x.AddRequestClient<NewLaptopFoundedEvent>();

                x.AddConsumer<NewLaptopFoundedEventHandler>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(rabbitOption.Host, rabbitOption.VirtualHost, h =>
                    {
                        h.Username(rabbitOption.Username);
                        h.Password(rabbitOption.Password);
                    });

                    config.ReceiveEndpoint(e =>
                    {
                        e.Durable = true;
                        e.ConfigureConsumer<NewLaptopFoundedEventHandler>(context);
                    });

                    config.ConfigureEndpoints(context);
                });
            }
            );
            //services.AddMassTransitHostedService();
            return services;
        }
    }
}
