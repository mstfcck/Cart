using System.Threading.Tasks;
using Cart.Application.Cart.Queries.GetCart;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shouldly;

namespace Cart.Application.Test;

public class GetCartQueryTests : ApplicationTests
{
    [TestCase]
    public async Task GetCartQueryTest()
    {
        var mediator = ServiceProvider.GetService<IMediator>();

        var response = await mediator.Send(new GetCartQuery());

        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }
}