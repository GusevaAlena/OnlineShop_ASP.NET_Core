using System;
using System.Collections.Generic;
using System.Linq;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public User User { get;set; }
        public decimal TotalPrice
        {
            get =>
                Positions.Sum(pos => pos.Product.Price * pos.Quantity);
        }
        public List<CartPosition> Positions { get; set; }
        public int TotalQuantity { get => Positions.Sum(pos => pos.Quantity); }
    }
}
