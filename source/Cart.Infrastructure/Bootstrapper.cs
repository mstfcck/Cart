using Cart.Domain;
using Cart.Domain.Repositories;
using Cart.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Cart.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<ICartRepository, RedisCartRepository>();
        return services;
    }
}