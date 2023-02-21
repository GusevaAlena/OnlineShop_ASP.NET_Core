using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using System.IO;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using OnlineShopWebApp.ViewModels;
using System.Linq;
using OnlineShop.Db;
using AutoMapper;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string userId;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        public UserProfileController(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IWebHostEnvironment appEnvironment, IOrderRepository orderRepository, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.appEnvironment = appEnvironment;
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.FindByIdAsync(userId);
            return View(mapper.Map<UserProfileViewModel>(user));
        }

        public async Task<IActionResult> EditAsync()
        {
            var user = await userManager.FindByIdAsync(userId);
            return View(mapper.Map<UserProfileViewModel>(user));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UserProfileViewModel userProfVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(userId);

                if (userProfVM.UploadedFile != null)
                {
                    string imagePath = Path.Combine(appEnvironment.WebRootPath + "/images/users/");
                    var fileName = Guid.NewGuid() + "." +
                       userProfVM.UploadedFile.FileName.Split('.').Last();

                    using (var fileStream = new FileStream(imagePath + fileName, FileMode.Create))
                    {
                        userProfVM.UploadedFile.CopyTo(fileStream);
                    }
                    userProfVM.ProfileImagePath = "/images/users/" + fileName;
                }

                if (user != null)
                {
                    user.Name = userProfVM.Name;
                    user.Surname = userProfVM.Surname;
                    user.PhoneNumber = userProfVM.PhoneNumber;
                    user.ProfileImagePath = userProfVM.ProfileImagePath;
                }

                await userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(userProfVM);
        }

        public async Task<IActionResult> Orders()
        {
            var userProfVM = new UserProfileViewModel();
            userProfVM.Orders = (await orderRepository
                .GetAllAsync())
                .Where(x =>x.Cart.User.Id==Guid.Parse(userId))
                .ToList();
             return View(userProfVM);
        }
    }
}
