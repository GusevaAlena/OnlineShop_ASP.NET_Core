using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShop.Db;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Helpers
{
    public class ProductsCache : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMemoryCache memoryCache;
        public ProductsCache(IMemoryCache memoryCache, IServiceProvider serviceProvider)
        {
            this.memoryCache = memoryCache;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                CachingAllProducts();
                await Task.Delay(TimeSpan.FromMilliseconds(60000), stoppingToken);
            }
        }

        private void CachingAllProducts()
        {
            using (var scope = serviceProvider.CreateAsyncScope())
            {
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var products = databaseContext.Products.ToList();
                
                if (products != null)
                {
                    memoryCache.Set("GetAllProducts",products);
                }

                foreach (var product in products)
                {
                    if (product != null)
                    {
                        memoryCache.Set(product.Id, product);
                    }
                }
            }
        }
    }
}
