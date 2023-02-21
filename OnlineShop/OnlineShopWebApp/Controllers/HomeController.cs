using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IMemoryCache memoryCache;

        public HomeController(IProductRepository productRepository, IMemoryCache memoryCache)
        {
            this.productRepository = productRepository;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            memoryCache.TryGetValue<List<Product>>("GetAllProducts", out var products);
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> SearchProductAsync(string searchString)
        {
            var products = await productRepository.SearchByNameAsync(searchString);
            if (products.Count == 0)
                return View("UnsuccessfulSearch");
            return View("Index", products);
        }
    }
}
