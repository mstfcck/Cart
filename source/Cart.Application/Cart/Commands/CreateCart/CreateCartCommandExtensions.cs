using Cart.Domain.Entities;

namespace Cart.Application.Cart.Commands.CreateCart;

public static class CreateCartCommandExtensions
{
    public static CartItem ToCartItem(this CreateCartCommand command)
    {
        return CartItem.Create(command.ProductId, "{ProductName}", 0, 1, "{PictureUrl}");
    }
}