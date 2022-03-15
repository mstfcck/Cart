using Cart.Domain;
using MediatR;

namespace Cart.Application.Cart.Queries.GetCart;

public class GetCartQuery : IRequest<GetCartDTO>
{
}