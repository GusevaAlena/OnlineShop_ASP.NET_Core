using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Db.Models
{
    public class User:IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfileImagePath { get; set; } = "/images/users/default.jpg";
        public List <Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
        public User()
        {
            Carts = new List<Cart>();
            Orders = new List<Order>();
        }
    }
}
