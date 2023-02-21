using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class OrderCreateViewModel
    {
        public int Id { get; set; }
        public UserViewModel User { get; set; }
        public AddressViewModel Address { get; set; }
        [Required(ErrorMessage = "Выберите способ оплаты")]
        public Payment Payment { get; set; }
        public IEnumerable<SelectListItem> PaymentMethodSelectList { get; set; }
    }
}
