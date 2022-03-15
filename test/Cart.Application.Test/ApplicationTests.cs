using System.Reflection;
using Cart.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using StackExchange.Redis;

namespace Cart.Application.Test;

[TestFixture]
public class ApplicationTests
{
    public ServiceProvider ServiceProvider { get; set; }

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(Bootstrapper));

        services.AddInfrastructure();

        services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var configurationOptions = ConfigurationOptions.Parse("localhost", true);
            configurationOptions.ResolveDns = true;
            return ConnectionMultiplexer.Connect(configurationOptions);
        });

        ServiceProvider = services.BuildServiceProvider();
    }
}