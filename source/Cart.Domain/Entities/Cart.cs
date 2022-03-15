namespace Cart.Domain.Entities;

public class Cart
{
    /// <summary>
    /// Key as "UserId"
    /// </summary>
    public string UserId { get; set; }
    public IList<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }

    public Cart(string userId)
    {
        UserId = userId;
        Items = new List<CartItem>();
    }

    /// <summary>
    /// Set UserId as Key
    /// </summary>
    /// <param name="userId"></param>
    public void SetUserId(string userId)
    {
        UserId = userId;
    }

    /// <summary>
    /// Add cart item to cart.
    /// </summary>
    /// <param name="cartItem"></param>
    public void AddCartItem(CartItem cartItem)
    {
        Items.Add(cartItem);
    }
}