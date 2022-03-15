namespace Cart.Application.Cart.Queries.GetCart;

public class GetCartDTO
{
    public GetCartDTO()
    {
        Items = new List<GetCartItemDTO>();
    }
    
    public IList<GetCartItemDTO> Items { get; set; }
}

public class GetCartItemDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
}