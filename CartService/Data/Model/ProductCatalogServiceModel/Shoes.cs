using System;

namespace CartService.Data.Model.ProductCatalogServiceModel
{
    public class Shoes : Item
    {
        // Shoe size (EU/US depends on context used by the app)
        public int Size { get; set; }

        public string Brand { get; set; } = string.Empty;
    }
}
