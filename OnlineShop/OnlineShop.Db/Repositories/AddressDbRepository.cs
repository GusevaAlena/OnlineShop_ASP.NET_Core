using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class AddressDbRepository : IAddressRepository
    {
        private readonly DatabaseContext databaseContext;
        public AddressDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task AddAsync(Address address)
        {
            await databaseContext.AddAsync(address);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Address>> GetAllAsync() =>
            await databaseContext.Addresses.ToListAsync();

        public Task<Address> TryGetByIdAsync(Guid id) =>
            databaseContext.Addresses.FirstOrDefaultAsync(a => a.Id == id);
    }
}
