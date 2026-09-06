using System;
using System.Collections.Generic;
using System.Linq;
using CartService.Data.Model;

namespace CartService.Data.ViewModel
{
    public class CartViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal Total => Items.Sum(i => i.LineTotal);

        public static CartViewModel FromModel(Cart cart)
        {
            return new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreatedAt,
                ExpiresAt = cart.ExpiresAt,
                Items = cart.CartItems.Select(CartItemViewModel.FromModel).ToList()
            };
        }
    }
}
