using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class StatusDbRepository : IStatusRepository
    {
        private readonly DatabaseContext databaseContext;
        public StatusDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<List<Status>> GetAllAsync() =>
            await databaseContext.Statuses.ToListAsync();

        public async Task<Status> TryGetByIdAsync(int id) =>
            await databaseContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);
    }
}
