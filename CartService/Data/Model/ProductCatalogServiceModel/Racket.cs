using System;

namespace CartService.Data.Model.ProductCatalogServiceModel
{
    public class Racket : Item
    {
        // Example specific properties for a racket
        public string GripSize { get; set; } = string.Empty;

        // Head size in square centimeters
        public double HeadSize { get; set; }
    }
}
