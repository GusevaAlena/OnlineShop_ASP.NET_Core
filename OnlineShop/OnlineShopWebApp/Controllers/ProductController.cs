using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IFavoriteRepository favoriteRepository;
        private readonly ICompareRepository compareRepository;
        private readonly IMemoryCache memoryCache;
        public readonly string userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IFavoriteRepository favoriteRepository, ICompareRepository compareRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache memoryCache)
        {
            this.productRepository = productRepository;
            this.favoriteRepository = favoriteRepository;
            this.compareRepository = compareRepository;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index(Guid id)
        {
            memoryCache.TryGetValue<Product>(id, out var product);
            return View(mapper.Map<ProductViewModel>(product));
        }

        [Authorize]
        public async Task<IActionResult> CompareAsync()
        {
            var compareProducts = await compareRepository.GelAllAsync(Guid.Parse(userId));
            if (compareProducts.Count == 0)
                return View("CompareEmpty");
            return View(mapper.Map<List<ProductViewModel>>(compareProducts));
        }

        [Authorize]
        public async Task<IActionResult> CompareAddAsync(Guid id)
        {
            await compareRepository.AddAsync(await productRepository.TryGetByIdAsync(id), Guid.Parse(userId));
            return View("Compare", mapper.Map<List<ProductViewModel>>(await compareRepository.GelAllAsync(Guid.Parse(userId))));
        }

        [Authorize]
        public async Task<IActionResult> CompareRemoveAsync(Guid id)
        {
            await compareRepository.RemoveAsync(await productRepository.TryGetByIdAsync(id), Guid.Parse(userId));
            var compareProducts = await compareRepository.GelAllAsync(Guid.Parse(userId));
            if (compareProducts.Count == 0)
                return View("CompareEmpty");
            return View("Compare", mapper.Map<List<ProductViewModel>>(compareProducts));
        }

        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            var favorites = await favoriteRepository.GetAllAsync(Guid.Parse(userId));
            if (favorites.Count == 0)
                return View("FavoritesEmpty");
            return View(mapper.Map<List<ProductViewModel>>(favorites));
        }

        [Authorize]
        public async Task<IActionResult> FavoriteAddAsync(Guid id)
        {
            await favoriteRepository.AddAsync(await productRepository.TryGetByIdAsync(id), Guid.Parse(userId));
            return View("Favorites", mapper.Map<List<ProductViewModel>>(await favoriteRepository.GetAllAsync(Guid.Parse(userId))));
        }

        [Authorize]
        public async Task<IActionResult> FavoriteRemoveAsync(Guid id)
        {
            await favoriteRepository.RemoveAsync(await productRepository.TryGetByIdAsync(id), Guid.Parse(userId));
            var favorites = await favoriteRepository.GetAllAsync(Guid.Parse(userId));
            if (favorites.Count == 0)
                return View("FavoritesEmpty");
            return View("Favorites", mapper.Map<List<ProductViewModel>>(favorites));
        }
    }
}
