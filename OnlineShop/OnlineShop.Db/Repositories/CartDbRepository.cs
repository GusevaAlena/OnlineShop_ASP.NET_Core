using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class CartDbRepository : ICartRepository
    {
        private readonly DatabaseContext databaseContext;
        public CartDbRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task<List<Cart>> GetAllAsync() => await databaseContext.Carts
            .Include(p=>p.CartPositions)
            .ThenInclude(p=>p.Product)
            .ToListAsync();

        public async Task<Cart> TryGetByUserIdAsync(Guid userId) =>
            await databaseContext.Carts.Include(x=>x.User)
            .Include(x => x.CartPositions)
            .ThenInclude(x => x.Product)
            .Where(x=>x.Enable==true)
            .FirstOrDefaultAsync(c => c.User.Id == userId);

        public async Task<Cart> TryGetByIdAsync(Guid Id) =>
           await databaseContext.Carts.Include(x => x.User)
            .Include(x => x.CartPositions)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(c => c.Id == Id);

        public async Task AddProductAsync(Product product, Guid userId)
        {
            var currentCart = await TryGetByUserIdAsync(userId);
            var currentUser = await databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (currentUser == null)
            {
                currentUser = new User { Id = userId };
                await databaseContext.Users.AddAsync(currentUser);
            }
            if (currentCart == null||currentCart.Enable==false)
            {
                currentCart = new Cart
                {
                    User = currentUser,
                    CartPositions = new List<CartPosition>
                    {
                        new CartPosition
                        {
                            Product = product,
                            Quantity=1
                        }
                    }
                };
                await databaseContext.Carts.AddAsync(currentCart);
            }
            else
            {
                var cartPosition = currentCart.CartPositions.FirstOrDefault(i => i.Product.Id == product.Id);
                if (cartPosition == null)
                {
                    currentCart.CartPositions.Add(new CartPosition
                    {
                        Product = product,
                        Quantity = 1,
                    });
                }
                else
                {
                    cartPosition.Quantity++;
                }
            }
            await databaseContext.SaveChangesAsync();
        }
        public async Task ClearAsync(Guid userId)
        {
            databaseContext.Remove(await TryGetByUserIdAsync(userId));
            await databaseContext.SaveChangesAsync();
        }

        public async Task IncreaseQuantityAsync(Product product, Guid userId)
        {
            var currentCart = await TryGetByUserIdAsync(userId);
            currentCart.CartPositions.FirstOrDefault(p => p.Product.Id == product.Id).Quantity++;
            await databaseContext.SaveChangesAsync();
        }

        public async Task DecreaseQuantityAsync(Product product, Guid userId)
        {
            var currentCart = await TryGetByUserIdAsync(userId);
            var currentPosition = currentCart?.CartPositions?.FirstOrDefault(p => p.Product.Id == product.Id);
            
            if (currentPosition == null)
                return;

            currentPosition.Quantity--;

            if (currentPosition.Quantity == 0)
            {
                currentCart?.CartPositions.Remove(currentPosition);
            }

            await databaseContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Cart cart)
        {
            var currentCart = await TryGetByIdAsync(cart.Id);
            currentCart.Id = cart.Id;
            currentCart.User=cart.User;
            currentCart.CartPositions = cart.CartPositions;
            currentCart.Enable = cart.Enable;
            await databaseContext.SaveChangesAsync();
        }
    }
}
