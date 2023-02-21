using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public interface IFavoriteRepository
    {
        Task<List<Product>> GetAllAsync(Guid userId);
        Task AddAsync(Product product, Guid userId);
        Task RemoveAsync(Product product, Guid userId);
    }
}
