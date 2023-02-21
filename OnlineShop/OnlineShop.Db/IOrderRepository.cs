using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IOrderRepository
    {
        Task<Order> TryGetByIdAsync(int id);
        Task<Order> TryGetByUserIdAsync(Guid userId);
        Task CreateAsync(Order order);
        Task UpdateStatusAsync(int orderId, int statusId);
        Task<List<Order>> GetAllAsync();
    }
}
