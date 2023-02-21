using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Db.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public List<CartPosition> CartPositions { get; set; }
        public bool Enable { get; set; }
        public Cart()
        {
            Enable = true;
            CartPositions = new List<CartPosition>();
        }
    }
}
