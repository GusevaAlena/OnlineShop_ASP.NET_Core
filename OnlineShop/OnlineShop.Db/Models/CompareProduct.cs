using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
    public class CompareProduct
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Product Product { get; set; }
    }
}
