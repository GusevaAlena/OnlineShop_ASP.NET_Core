using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface ICompareRepository
    {
        Task<List<Product>> GelAllAsync(Guid userId);
        Task AddAsync(Product product, Guid userId);
        Task RemoveAsync(Product product, Guid userId);
    }
}