using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShop.Db.Repositories;
using OnlineShopWebApp.Helpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Views.Shared.Components.UserProfile
{
    public class UserProfileViewComponent:ViewComponent
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string userId;
        private readonly IMemoryCache cache;
        public UserProfileViewComponent(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.cache = cache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName = User.Identity.Name;
            string profileImage = null;

            if (!cache.TryGetValue(userName, out profileImage))
            {
                var user = await userManager.FindByNameAsync(userName);
                profileImage = user.ProfileImagePath;

                if (profileImage != null)
                {
                    cache.Set(user.UserName, profileImage, 
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));
                }
            }
            return View("UserProfile",profileImage);
        }
    }
}
