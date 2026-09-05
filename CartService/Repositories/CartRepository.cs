using System;
using System.Threading.Tasks;
using CartService.Data;
using CartService.Data.Model;
using CartService.Data.Model.ProductCatalogServiceModel;
using Microsoft.EntityFrameworkCore;

namespace CartService.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _db;

        public CartRepository(CartDbContext db)
        {
            _db = db;
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Item?> GetItemByIdAsync(Guid itemId)
        {
            return await _db.Items.FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _db.Carts.AddAsync(cart);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
