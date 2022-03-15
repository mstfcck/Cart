namespace Cart.Domain.Repositories;

public interface ICartRepository
{
    Task<Entities.Cart?> GetAsync(string key);
    Task<bool> UpdateAsync(Entities.Cart cart);
    Task<bool> DeleteAsync(string key);
}