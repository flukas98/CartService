using CartService.Data.Model.IdentityServiceModel;
using CartService.Data.Model.ProductCatalogServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CartService.Data.Model
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Foreign key to User
        public Guid UserId { get; set; }

        public User? User { get; set; }

        // Many-to-many via join entity
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Convenience accessor returning items in the cart
        [NotMapped]
        public IEnumerable<Item> Items => CartItems?.Select(ci => ci.Item!).Where(i => i != null) ?? Enumerable.Empty<Item>();

        public void AddItem(Item item, int quantity = 1)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var existing = CartItems.FirstOrDefault(ci => ci.ItemId == item.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                CartItems.Add(new CartItem { CartId = this.Id, ItemId = item.Id, Item = item, Quantity = quantity });
            }
        }

        public void RemoveItem(Guid itemId)
        {
            var existing = CartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (existing == null) return;

            if (existing.Quantity > 1)
            {
                existing.Quantity--;
            }
            else
            {
                CartItems.Remove(existing);
            }
        }
    }
}
