namespace Cart.Domain.Entities;

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }

    #region Domain Logics

    /// <summary>
    /// Add cart item.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productName"></param>
    /// <param name="unitPrice"></param>
    /// <param name="quantity"></param>
    /// <param name="pictureUrl"></param>
    /// <returns></returns>
    public static CartItem Create(int productId, string productName, decimal unitPrice, int quantity, string pictureUrl)
    {
        return new CartItem
        {
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            PictureUrl = pictureUrl
        };
    }

    /// <summary>
    /// Increase the cart item quantity.
    /// </summary>
    public void IncreaseQuantityOne()
    {
        Quantity++;
    }
    
    /// <summary>
    /// Decrease the cart item quantity.
    /// </summary>
    public void DecreaseQuantityOne()
    {
        Quantity--;
    }

    #endregion
}