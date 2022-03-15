using Cart.Api.Models.Requests;
using Cart.Api.Models.Responses;
using Cart.Application.Cart.Commands;
using Cart.Application.Cart.Commands.CreateCart;
using Cart.Application.Cart.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Controllers;

[ApiController]
[Route("carts")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Add cart.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task AddCart([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateCartCommand
        {
            ProductId = request.ProductId
        }, cancellationToken);
    }

    /// <summary>
    /// Get cart.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetCartResponse), StatusCodes.Status200OK)]
    public async Task<GetCartResponse?> GetCart(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCartQuery(), cancellationToken);
        return new GetCartResponse
        {
            Items = response.Items?.Select(x => new GetCartItemResponse
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                PictureUrl = x.PictureUrl
            }).ToList()
        };
    }
}