namespace MarsCommerce.Core.Models
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }
        public required string StockKeepingUnit { get; set; }
        public int? Rating { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual List<ProductAttributeMapping>? Attributes { get; set; }
        public virtual List<ShoppingCartItem>? Items { get; set; }


    }
}
