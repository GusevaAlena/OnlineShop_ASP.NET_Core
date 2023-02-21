using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
