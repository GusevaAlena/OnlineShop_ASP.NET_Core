using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Repositories
{
    public class FavoriteDbRepository:IFavoriteRepository
    {
        private readonly DatabaseContext databaseContext;
        public FavoriteDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task AddAsync(Product product, Guid userId)
        {
            var favoriteProduct = await databaseContext.FavoriteProducts
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Product == product);
            
            if (favoriteProduct == null)
            {
                favoriteProduct=new FavoriteProduct 
                { 
                    UserId = userId,
                    Product = product 
                };
                await databaseContext.FavoriteProducts.AddAsync(favoriteProduct);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllAsync(Guid userId)=>     
            await databaseContext.FavoriteProducts.Where(p => p.UserId == userId)
            .Include(p=>p.Product)
            .Select(p=>p.Product)
            .ToListAsync();
        
        public async Task RemoveAsync(Product product, Guid userId)
        {
            var favoriteProduct = await databaseContext.FavoriteProducts
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Product == product);
            if (favoriteProduct != null)
            {
                databaseContext.FavoriteProducts.Remove(favoriteProduct);
                await databaseContext.SaveChangesAsync();
            }
        }        
    }
}
