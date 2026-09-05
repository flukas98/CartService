namespace CartService.Data.Model.InventoryServiceModel
{
    public class ItemQuantity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ItemId { get; set; }

        public int Quantity { get; set; }
    }
}
