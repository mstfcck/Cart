using System.Text.Json;
using Cart.Domain.Repositories;
using StackExchange.Redis;

namespace Cart.Infrastructure.Repositories;

public class RedisCartRepository : ICartRepository
{
    private readonly IDatabase _database;

    public RedisCartRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<Domain.Entities.Cart?> GetAsync(string key)
    {
        var cart = await _database.StringGetAsync(key);
        return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Domain.Entities.Cart>(cart);
    }
    
    public async Task<bool> UpdateAsync(Domain.Entities.Cart cart)
    {
        return await _database.StringSetAsync(cart.UserId, JsonSerializer.Serialize(cart));
    }
    
    public async Task<bool> DeleteAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }
}