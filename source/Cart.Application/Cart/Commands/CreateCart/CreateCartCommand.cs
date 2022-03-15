using MediatR;

namespace Cart.Application.Cart.Commands.CreateCart;

public class CreateCartCommand : IRequest
{
    public int ProductId { get; set; }
}