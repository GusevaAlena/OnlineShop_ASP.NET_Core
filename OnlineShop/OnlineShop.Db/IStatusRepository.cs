using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetAllAsync();
        Task<Status> TryGetByIdAsync(int id);
    }
}
