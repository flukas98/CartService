using System;
using System.Threading.Tasks;
using CartService.Data.Model.ProductCatalogServiceModel;
using CartService.Data.Model;

namespace CartService.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(Guid userId);

        Task<Item?> GetItemByIdAsync(Guid itemId);

        Task AddCartAsync(Cart cart);

        Task SaveChangesAsync();
    }
}
