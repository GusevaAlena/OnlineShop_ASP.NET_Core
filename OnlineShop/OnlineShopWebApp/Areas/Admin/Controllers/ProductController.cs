using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Администратор")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.appEnvironment = appEnvironment;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await productRepository.GetAllAsync();
            return View(mapper.Map<List<ProductViewModel>>(products));
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ProductViewModel productVM)
        {
            if (productVM.Price != Math.Truncate(productVM.Price))
            {
                ModelState.AddModelError("", "Цена должна быть целым числом");
            }

            if (ModelState.IsValid)
            {
                if (productVM.UploadedFile != null)
                {
                    string imagePath = Path.Combine(appEnvironment.WebRootPath + "/images/Products/");

                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    var fileName = Guid.NewGuid() + "." +
                        productVM.UploadedFile.FileName.Split('.').Last();

                    using (var fileStream = new FileStream(imagePath + fileName, FileMode.Create))
                    {
                        productVM.UploadedFile.CopyTo(fileStream);
                    }
                    productVM.ImagePath = "/images/Products/" + fileName;
                }
                
                await productRepository.AddAsync(mapper.Map<Product>(productVM));
                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        public async Task<IActionResult> RemoveAsync(Guid id)
        {
            await productRepository.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditAsync(Guid id)
        {
            var product = await productRepository.TryGetByIdAsync(id);
            return View("Edit", mapper.Map<ProductViewModel>(product));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(ProductViewModel productVM)
        {
            if (productVM.Price != Math.Truncate(productVM.Price))
            {
                ModelState.AddModelError("", "Цена должна быть целым числом");
            }

            if (ModelState.IsValid)
            {
                if (productVM.UploadedFile != null)
                {
                    string imagePath = Path.Combine(appEnvironment.WebRootPath + "/images/Products/");
                    var fileName = Guid.NewGuid() + "." +
                       productVM.UploadedFile.FileName.Split('.').Last();
                    using (var fileStream = new FileStream(imagePath + fileName, FileMode.Create))
                    {
                        productVM.UploadedFile.CopyTo(fileStream);
                    }
                    productVM.ImagePath = "/images/Products/" + fileName;
                }
                await productRepository.UpdateAsync(mapper.Map<Product>(productVM));
                return RedirectToAction("Index");
            }
            return View(productVM);
        }
    }
}
