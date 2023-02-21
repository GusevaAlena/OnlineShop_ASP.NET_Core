using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using System.Text.RegularExpressions;

namespace OnlineShop.Db.Repositories
{
    public class ProductDbRepository : IProductRepository
    {
        private readonly DatabaseContext databaseContext;
        public ProductDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
   
        public async Task<List<Product>> GetAllAsync() => 
            await databaseContext.Products.ToListAsync();

        public async Task<Product> TryGetByIdAsync(Guid id) => 
            await databaseContext.Products.FirstOrDefaultAsync(product => product.Id == id);

        public async Task AddAsync(Product product)
        {
            await databaseContext.Products.AddAsync(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var product = await TryGetByIdAsync(id);
            databaseContext.Products.Remove(product);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Product>> SearchByNameAsync(string inputString) =>
            (await databaseContext.Products.ToListAsync()).FindAll(p => Regex.IsMatch(p.Name, inputString, RegexOptions.IgnoreCase));

        public async Task UpdateAsync(Product product)
        {
            var currentProduct = await TryGetByIdAsync(product.Id);
            currentProduct.Id = product.Id;
            currentProduct.Name = product.Name;
            currentProduct.Price = product.Price;
            currentProduct.TotalQuantity = product.TotalQuantity;
            currentProduct.Description = product.Description;
            currentProduct.ImagePath = product.ImagePath;
            await databaseContext.SaveChangesAsync();
        }        
    }
}