using System.Threading.Tasks;
using Cart.Application.Cart.Commands;
using Cart.Application.Cart.Commands.CreateCart;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace Cart.Application.Test;

[TestFixture]
public class CreateCartCommandTests : ApplicationTests
{
    [TestCase]
    public async Task CreateCartCommandValidatedTest()
    {
        var mediator = ServiceProvider.GetService<IMediator>();

        var response = await mediator.Send(new CreateCartCommand {ProductId = 1});

        response.ShouldBe(Unit.Value);
    }
    
    [TestCase]
    public async Task CreateCartCommandUnValidatedTest()
    {
        var mediator = ServiceProvider.GetService<IMediator>();

        var response = await mediator.Send(new CreateCartCommand {ProductId = 0});

        response.ShouldNotBeOfType<ValidationException>();
    }
}