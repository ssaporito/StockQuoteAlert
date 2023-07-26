using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Messaging;
public static class IServiceCollectionExtensions
{

    public static IServiceCollection SetUpRabbitMq(this IServiceCollection services, IConfiguration config)
    {
        var configSection = config.GetSection("RabbitMQSettings");
        var settings = new RabbitMQSettings();
        configSection.Bind(settings);

        // add the settings for later use by other classes via injection
        services.AddSingleton(settings);
        services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
        {
            HostName = settings.HostName,
            DispatchConsumersAsync = true
        });

        services.AddSingleton<ModelFactory>();
        services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());
        return services;
    }    
}