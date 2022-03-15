using Cart.Api.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Cart.Api.Extensions;

public static class RedisExtensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("Settings:RedisSettings"));

        services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<RedisSettings>>().Value;
            var configurationOptions = ConfigurationOptions.Parse(settings.ConnectionString, true);
            configurationOptions.ResolveDns = true;
            return ConnectionMultiplexer.Connect(configurationOptions);
        });
    }
}