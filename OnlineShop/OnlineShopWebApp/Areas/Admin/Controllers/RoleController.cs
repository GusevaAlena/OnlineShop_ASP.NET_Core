using AutoMapper;
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
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<Role> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(mapper.Map<List<RoleViewModel>>(roles));
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(RoleViewModel roleVM)
        {
            if (roleManager.Roles.ToList().Find(r => r.Name == roleVM.Name) != null)
            {
                ModelState.AddModelError("", "Такая роль уже существует");
            }

            if (ModelState.IsValid)
            {
                await roleManager.CreateAsync(mapper.Map<Role>(roleVM));
                return RedirectToAction("Index");
            }
            return View(roleVM);
        }
        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            var role = roleManager.Roles.ToList().FirstOrDefault(x=>x.Id==id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}
