using OnlineShop.Db.Models;
using System.Collections;
using System.Collections.Generic;

namespace OnlineShop.Db
{
    public interface IProductRepository
    {
        Task<Product> TryGetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task RemoveAsync(Guid id);
        Task<List<Product>> SearchByNameAsync(string inputString);
        Task UpdateAsync(Product product);
    }
}
