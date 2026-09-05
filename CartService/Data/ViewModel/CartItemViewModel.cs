using System;
using CartService.Data.Model;

namespace CartService.Data.ViewModel
{
    public class CartItemViewModel
    {
        public Guid ItemId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal LineTotal => Price * Quantity;

        public static CartItemViewModel FromModel(CartItem cartItem)
        {
            return new CartItemViewModel
            {
                ItemId = cartItem.ItemId,
                Name = cartItem.Item?.Name ?? string.Empty,
                Price = cartItem.Item?.Price ?? 0,
                Quantity = cartItem.Quantity
            };
        }
    }
}
