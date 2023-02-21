using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class CartPosition
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }   
    }
}
