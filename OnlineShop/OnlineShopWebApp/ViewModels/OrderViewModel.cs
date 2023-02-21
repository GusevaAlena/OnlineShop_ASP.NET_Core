using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public CartViewModel Cart { get; set; }
        public Address Address { get; set; }
        public Status Status { get; set; }
        public Payment Payment { get; set; }
        public DateTime CreationDateTime { get; set; }
        public IEnumerable<SelectListItem> StatusSelectList { get; set; }
    }
}
