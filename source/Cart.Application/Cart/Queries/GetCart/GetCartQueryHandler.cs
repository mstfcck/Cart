using Cart.Domain.Repositories;
using MediatR;

namespace Cart.Application.Cart.Queries.GetCart;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, GetCartDTO>
{
    private readonly ICartRepository _cartRepository;

    public GetCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<GetCartDTO> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var key = "{UserId}"; // Get {UserId} from IHttpContextAccessor
        
        var cart = await _cartRepository.GetAsync(key);

        if (cart == null)
        {
            
        }

        var response = new GetCartDTO
        {
            Items = cart?.Items.Select(x => new GetCartItemDTO
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                PictureUrl = x.PictureUrl
            }).ToList()
        };

        return response;
    }
}