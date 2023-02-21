using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using OnlineShopWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Администратор")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View(mapper.Map<List<UserViewModel>>(users));
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = userVM.Name,
                    Surname = userVM.Surname,
                    Email = userVM.Email,
                    PhoneNumber = userVM.PhoneNumber,
                    UserName = userVM.Email
                };

                await userManager.CreateAsync(user);
                var userAdded = await userManager.FindByIdAsync(user.Id.ToString());

                if (userAdded != null)
                    return RedirectToAction("Index");
            }
            return View(userVM);
        }
        public async Task<IActionResult> Details(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return View(mapper.Map<UserViewModel>(user));
        }
        public async Task<IActionResult> RemoveAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return NotFound();

            return View(mapper.Map<UserViewModel>(user));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.FindByIdAsync(userVM.Id.ToString());

                if (currentUser != null)
                {
                    currentUser.Name = userVM.Name;
                    currentUser.Email = userVM.Email;
                    currentUser.Surname = userVM.Surname;
                    currentUser.PhoneNumber = userVM.PhoneNumber;
                }

                await userManager.UpdateAsync(currentUser);
                return RedirectToAction("Index");
            }
            return View(userVM);
        }

        public async Task<IActionResult> ChangePasswordAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return NotFound();

            var rVM = new RegisterViewModel();
            rVM.Email = user.Email;
            return View(rVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(RegisterViewModel rVM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(rVM.Email);
                if (user != null)
                {
                    var passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                    var result = await passwordValidator.ValidateAsync(userManager, user, rVM.Password);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, rVM.Password);
                        await userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(rVM);
        }
        public async Task<IActionResult> GiveRoleAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var userRoles = await userManager.GetRolesAsync(user);
            var allRoles = await roleManager.Roles.ToListAsync();
            var giveRoleVM = new GiveRoleViewModel
            {
                UserName = user.UserName,
                UserRoles = userRoles.Select(x => new RoleViewModel { Name = x }).ToList(),
                AllRoles = allRoles.Select(x => new RoleViewModel { Name = x.Name }).ToList()
            };
            return View(giveRoleVM);
        }

        [HttpPost]
        public async Task<IActionResult> GiveRoleAsync(GiveRoleViewModel giveRoleVM, List<string> selectedRoles)
        {
            var user = await userManager.FindByNameAsync(giveRoleVM.UserName);
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles);
            await userManager.AddToRolesAsync(user, selectedRoles);
            return RedirectToAction("Details", new { userId = user.Id });
        }
    }
}
