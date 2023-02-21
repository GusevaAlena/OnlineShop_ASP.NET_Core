using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IPaymentMethodRepository
    {
        Task<Payment> TryGetByIdAsync(int id);
        Task<List<Payment>> GetAllAsync();
    }
}
