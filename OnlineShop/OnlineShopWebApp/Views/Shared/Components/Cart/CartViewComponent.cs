using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Views.Shared.Components.Cart
{
    public class CartViewComponent:ViewComponent
    {
        private readonly ICartRepository cartRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string userId;
        private readonly IMapper mapper;

        public CartViewComponent(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.cartRepository = cartRepository;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User
                 .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                var cookies = httpContextAccessor.HttpContext.Request.Cookies;
                if (cookies.ContainsKey("CurrentUserId"))
                {
                    userId = cookies["CurrentUserId"];
                }
                else
                {
                    userId = Guid.NewGuid().ToString();
                    httpContextAccessor.HttpContext.Response.Cookies.Append("CurrentUserId", userId);
                }
            }

            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        { 
            var cart = await cartRepository.TryGetByUserIdAsync(Guid.Parse(userId));
            var totalQuantity = mapper.Map<CartViewModel>(cart)?.TotalQuantity ?? 0;
            if (cart?.Enable == false)
                totalQuantity = 0;
            return View("Cart",totalQuantity);  
        }
    }
}
