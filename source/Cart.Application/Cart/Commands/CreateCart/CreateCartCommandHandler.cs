using Cart.Domain.Repositories;
using MediatR;

namespace Cart.Application.Cart.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand>
{
    private readonly ICartRepository _cartRepository;

    public CreateCartCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Unit> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        /*
         * TODO:
         * - Get product details from Product API
         * - Check stock status from Stock API
         *
         * There are two approaches to do that:
         * 
         * First of all, the goal is that Product API and Stock API then we get the product details and check the available stock status.
         * 
         * To do that, we can do a direct (HTTP) call from here to other APIs or we crate atomic endpoints
         * to provide the data then create a BFF API and then get product details, check the available stock status,
         * and add to the cart from BFF.
         */

        var key = "{UserId}";
        
        var cart = await _cartRepository.GetAsync(key);

        if (cart == null)
        {
            cart = new Domain.Entities.Cart(key);
            
            cart.AddCartItem(request.ToCartItem());
        }
        else
        {
            var cartItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (cartItem == null)
            {
                cart.AddCartItem(request.ToCartItem());
            }
            else
            {
                cartItem.IncreaseQuantityOne();
            }
        }

        await _cartRepository.UpdateAsync(cart);

        return Unit.Value;
    }
}