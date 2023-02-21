using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string Method { get; set; }
    }
}
