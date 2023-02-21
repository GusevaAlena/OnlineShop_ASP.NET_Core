using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly string userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICartRepository cartRepository;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, ICartRepository cartRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                userId = httpContextAccessor.HttpContext.Request.Cookies["CurrentUserId"];
            }

            this.cartRepository = cartRepository;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            var user = await userManager.FindByNameAsync(loginVM.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователя с таким логином не существует");
            }

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync
                    (loginVM.UserName, loginVM.Password, loginVM.RememberMe, false);

                if (result.Succeeded)
                {
                    await CopyCartFromUnAuthUserToAuthUserAsync(user.Id);
                    return Redirect(loginVM.ReturnUrl ?? "/Home");
                }

                ModelState.AddModelError("", "Неправильно введен пароль");
            }

            return View(loginVM);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerVM)
        {
            if (await userManager.FindByNameAsync(registerVM.Email) != null)
            {
                ModelState.AddModelError("", "Пользователь с таким e-mail уже существует");
            }

            if (ModelState.IsValid)
            {
                var user = new User { Email = registerVM.Email, UserName = registerVM.Email };
                var result = await userManager.CreateAsync(user, registerVM.Password);
                
                if (result.Succeeded)
                {
                    await TryAssignUserRoleAsync(user);
                    await signInManager.SignInAsync(user, false);
                }
                return Redirect(registerVM.ReturnUrl ?? "/Home");
            }
            return View(registerVM);
        }

        private async Task TryAssignUserRoleAsync(User user)
        {
            try
            {
                await userManager.AddToRoleAsync(user, "Пользователь");
            }
            catch { }
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task CopyCartFromUnAuthUserToAuthUserAsync(Guid authUserId)
        {
            var currentCart = await cartRepository.TryGetByUserIdAsync(Guid.Parse(userId));
            if (currentCart != null)
            {
                currentCart.User = await userManager.FindByIdAsync(authUserId.ToString());
                await cartRepository.UpdateAsync(currentCart);
                httpContextAccessor.HttpContext.Response.Cookies.Delete("CurrentUserId");
            }
            else return;
        }
    }
}
