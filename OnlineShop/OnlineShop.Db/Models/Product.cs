using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int TotalQuantity { get; set; } 
        public string ImagePath { get; set; }
        public List<CartPosition> CartPositions { get; set; }
        public List<FavoriteProduct> FavoriteProducts { get; set; }
        public List<CompareProduct> CompareProducts { get; set; }
        public Product()
        {
            CartPositions = new List<CartPosition>();
            FavoriteProducts = new List<FavoriteProduct>();
            CompareProducts = new List<CompareProduct>();
        }

        public Product(string id,string name, decimal price, string description, int totalQuantity, string imagePath)
        {
            Id= new Guid(id);
            Name = name;
            Price = price;
            Description = description;
            TotalQuantity = totalQuantity;
            ImagePath = imagePath;
        }
    }
}
