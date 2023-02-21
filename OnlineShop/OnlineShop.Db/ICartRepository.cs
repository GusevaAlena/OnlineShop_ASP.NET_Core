using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetAllAsync();
        Task<Cart> TryGetByUserIdAsync(Guid userId);
        Task<Cart> TryGetByIdAsync(Guid Id);
        Task AddProductAsync(Product product, Guid userId);
        Task ClearAsync(Guid userId);
        Task IncreaseQuantityAsync(Product product, Guid userId);
        Task DecreaseQuantityAsync(Product product, Guid userId);
        Task UpdateAsync(Cart cart);
    }
}
