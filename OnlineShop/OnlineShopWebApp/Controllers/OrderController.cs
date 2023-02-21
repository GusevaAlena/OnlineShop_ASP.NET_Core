using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;
        private readonly UserManager<User> userManager;
        private readonly IPaymentMethodRepository paymentRepository;
        private readonly IAddressRepository addressRepository;
        private readonly string userId;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        public OrderController(IOrderRepository orderRepository,
            ICartRepository cartRepository, IPaymentMethodRepository payments,
            IAddressRepository addressRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
            paymentRepository = payments;
            this.addressRepository = addressRepository;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var currentUser = await userManager.FindByIdAsync(userId);
            var orderModel = new OrderCreateViewModel();
            orderModel.User = mapper.Map<UserViewModel>(currentUser);
            orderModel.PaymentMethodSelectList = (await paymentRepository.GetAllAsync())
                .Select(p =>
                new SelectListItem { Value = p.Id.ToString(), Text = p.Method });
            return View(orderModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(OrderCreateViewModel orderModel)
        {
            if (ModelState.IsValid)
            {
                orderModel.User.Id = Guid.Parse(userId);
                await UpdateUsersDataAsync(orderModel.User);
                await addressRepository.AddAsync(mapper.Map<Address>(orderModel.Address));
                var order = mapper.Map<Order>(orderModel);
                var cart = await cartRepository.TryGetByUserIdAsync(Guid.Parse(userId));
                order.Cart = cart;
                order.Payment = await paymentRepository.TryGetByIdAsync(orderModel.Payment.Id);
                order.Cart.Enable = false;
                await orderRepository.CreateAsync(order);
                var orderId = (await orderRepository.TryGetByUserIdAsync(Guid.Parse(userId))).Id;
                return RedirectToAction("Success", new { id = orderId });
            }
            orderModel.PaymentMethodSelectList = (await paymentRepository.GetAllAsync())
                .Select(p =>
                new SelectListItem { Value = p.Id.ToString(), Text = p.Method });
            return View(orderModel);
        }
        public async Task UpdateUsersDataAsync(UserViewModel userVM)
        {
            var currentUser = await userManager.FindByIdAsync(userVM.Id.ToString());
            
            if (currentUser != null)
            {
                currentUser.Name = userVM.Name;
                currentUser.Surname = userVM.Surname;
                currentUser.PhoneNumber = userVM.PhoneNumber;
            }
            
            await userManager.UpdateAsync(currentUser);
        }
        public IActionResult Success(int? id)
        {
            return View(id);
        }
    }
}
