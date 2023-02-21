using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class CompareDbRepository:ICompareRepository
    {
        private readonly DatabaseContext databaseContext;
        public CompareDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task AddAsync(Product product, Guid userId)
        {
            var compareProduct = await databaseContext.CompareProducts
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Product == product);
            if (compareProduct == null)
            {
                compareProduct = new CompareProduct 
                { 
                    UserId = userId,
                    Product = product 
                };
                await databaseContext.CompareProducts.AddAsync(compareProduct);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GelAllAsync(Guid userId)=>     
            await databaseContext.CompareProducts.Where(p => p.UserId == userId)
            .Include(p=>p.Product)
            .Select(p=>p.Product)
            .ToListAsync();
        
        public async Task RemoveAsync(Product product, Guid userId)
        {
            var compareProduct = await databaseContext.CompareProducts
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Product == product);
            if (compareProduct != null)
            {
                databaseContext.CompareProducts.Remove(compareProduct);
                await databaseContext.SaveChangesAsync();
            }
        }        
    }
}
