using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public interface IAddressRepository
    {
        Task AddAsync(Address address);
        Task<List<Address>> GetAllAsync();
        Task<Address> TryGetByIdAsync(Guid id);
    }
}
