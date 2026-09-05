using System;

namespace CartService.Data.Model.ProductCatalogServiceModel
{
    public abstract class Item
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public virtual string? Description { get; set; }
    }
}
