namespace OnlineShop.Db.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int EntranceNumber { get; set; }
        public int Floor { get; set; }
        public int FlatNumber { get; set; }
        public List<Order> Orders { get; set; }
        public Address()
        {
            Orders = new List<Order>();
        }
    }
}
