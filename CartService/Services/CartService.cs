using System;
using System.Linq;
using System.Threading.Tasks;
using CartService.Data.Model;
using CartService.Data.Model.ProductCatalogServiceModel;
using CartService.Repositories;

namespace CartService.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;

        public CartService(ICartRepository repo)
        {
            _repo = repo;
        }

        public async Task<Cart?> GetCartAsync(Guid userId)
        {
            if (userId == Guid.Empty) return null;

            return await _repo.GetCartByUserIdAsync(userId);
        }

        public async Task<bool> AddItemToUserCartAsync(Guid userId, Guid itemId, int quantity = 1)
        {
            if (userId == Guid.Empty) return false;

            var cart = await _repo.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _repo.AddCartAsync(cart);
            }

            // call towards InventoryService to check if Item's quantity is > 0
            // if so, item is added into cart, otherwise return false
            var item = await _repo.GetItemByIdAsync(itemId);
            if (item == null) return false;

            cart.AddItem(item, quantity);

            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemFromUserCartAsync(Guid userId, Guid itemId)
        {
            if (userId == Guid.Empty) return false;

            var cart = await _repo.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            if (!cart.CartItems.Any(ci => ci.ItemId == itemId)) return false;

            cart.RemoveItem(itemId);

            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCartAsync(Guid userId)
        {
            if (userId == Guid.Empty) return false;

            var cart = await _repo.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            await _repo.RemoveCartAsync(cart);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
