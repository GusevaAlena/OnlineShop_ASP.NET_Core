using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public Cart Cart { get; set; }
        public Status Status { get; set; }
        public Address Address { get; set; }
        public Payment Payment { get; set; }
        public List<Status> Statuses { get; set; }
        public Order()
        {
            CreationDateTime= DateTime.Now;
            Statuses = new List<Status>();
        }      
    }
}
