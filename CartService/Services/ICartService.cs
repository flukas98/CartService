using System;
using System.Threading.Tasks;
using CartService.Data.Model;

namespace CartService.Services
{
    public interface ICartService
    {
        Task<bool> AddItemToUserCartAsync(Guid userId, Guid itemId, int quantity = 1);

        Task<bool> RemoveItemFromUserCartAsync(Guid userId, Guid itemId);

        Task<Cart?> GetCartAsync(Guid userId);
    }
}
