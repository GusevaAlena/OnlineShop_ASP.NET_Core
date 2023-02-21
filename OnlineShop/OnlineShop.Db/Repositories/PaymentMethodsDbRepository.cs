using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class PaymentMethodsDbRepository:IPaymentMethodRepository
    {
       private readonly DatabaseContext databaseContext;
        public PaymentMethodsDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<Payment> TryGetByIdAsync(int id) =>
            await databaseContext.Payments.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<List<Payment>> GetAllAsync() => 
            await databaseContext.Payments.ToListAsync();
    }
}
