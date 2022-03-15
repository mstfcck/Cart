namespace Cart.Api.Models.Responses;

public class GetCartResponse
{
    public List<GetCartItemResponse>? Items { get; set; }
}

public class GetCartItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
}