using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class OrderDbRepository : IOrderRepository
    {
        private readonly DatabaseContext databaseContext;
        public OrderDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<Order> TryGetByUserIdAsync(Guid userId) =>
           await databaseContext.Orders.Include(x=>x.Cart)
            .ThenInclude(x=>x.User)
            .OrderBy(d=>d.CreationDateTime)
            .LastAsync(o => o.Cart.User.Id == userId);
      
        public async Task<Order> TryGetByIdAsync(int id) =>
           await databaseContext.Orders
            .Include(o=>o.Address)          
            .Include(o=>o.Payment)
            .Include(o=>o.Status)
            .Include(o => o.Cart)
            .ThenInclude(c=>c.User)
            .Include(o=>o.Cart)
            .ThenInclude(c=>c.CartPositions)
            .ThenInclude(p=>p.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        public async Task CreateAsync(Order order)
        {
            order.Status = await databaseContext.Statuses.FirstOrDefaultAsync(s => s.Name == "Создан");
            await databaseContext.Orders.AddAsync(order);
            await databaseContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int orderId, int statusId)
        {
            var order = await TryGetByIdAsync(orderId);
            var status = await databaseContext.Statuses.FirstOrDefaultAsync(s=>s.Id==statusId);
            order.Status = status;
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync() => await databaseContext.Orders
            .Include(o => o.Address)
            .Include(o => o.Payment)
            .Include(o => o.Status)
            .Include(o => o.Cart)
            .ThenInclude(c => c.User)
            .Include(o => o.Cart)
            .ThenInclude(c => c.CartPositions)
            .ThenInclude(p => p.Product).ToListAsync();
    }
}
