using System;

namespace CartService.Data.Model.ProductCatalogServiceModel
{
    public class Ball : Item
    {
        // Example specific properties for a ball
        public string Material { get; set; } = string.Empty;

        // Diameter in centimeters
        public double Diameter { get; set; }
    }
}
