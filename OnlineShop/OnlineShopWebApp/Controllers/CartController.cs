using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShopWebApp.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICartRepository cartRepository;
        private readonly string userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public CartController(IProductRepository productRepository, ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                userId = httpContextAccessor.HttpContext.Request.Cookies["CurrentUserId"];
            }

            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentCart = await cartRepository.TryGetByUserIdAsync(Guid.Parse(userId));
            if (currentCart != null)
                if (currentCart.CartPositions.Count != 0 && currentCart.Enable == true)                              
                    return View(mapper.Map<CartViewModel>(currentCart));
            return View("Empty");
        }
        

        public async Task<IActionResult> AddProductAsync(Guid productId)
        {
            await cartRepository.AddProductAsync(await productRepository.TryGetByIdAsync(productId), Guid.Parse(userId));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearAsync(Guid userId)
        {
            await cartRepository.ClearAsync(userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseQuantityAsync(Guid productId, Guid userId)
        {
            await cartRepository.IncreaseQuantityAsync(await productRepository.TryGetByIdAsync(productId), userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseQuantityAsync(Guid productId, Guid userId)
        {
            await cartRepository.DecreaseQuantityAsync(await productRepository.TryGetByIdAsync(productId), userId);
            return RedirectToAction("Index");
        }
    }
}
