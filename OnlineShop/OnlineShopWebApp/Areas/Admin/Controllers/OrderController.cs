using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Администратор")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMapper mapper;

        public OrderController(IOrderRepository orderRepository, IStatusRepository statusRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await orderRepository.GetAllAsync();
            return View(mapper.Map<List<OrderViewModel>>(orders));
        }
        public async Task<IActionResult> Details(int orderId)
        {
            var orderVM = new OrderViewModel();
            orderVM = mapper.Map<OrderViewModel>(await orderRepository.TryGetByIdAsync(orderId));
            orderVM.StatusSelectList = (await statusRepository.GetAllAsync()).Select(s =>
                new SelectListItem { Value = s.Id.ToString(), Text = s.Name });
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusAsync(OrderViewModel orderVM, int orderId)
        {
            var statusId = orderVM.Status.Id;
            await orderRepository.UpdateStatusAsync(orderId, statusId);
            return RedirectToAction("Index");
        }
    }
}
