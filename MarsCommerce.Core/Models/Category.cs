namespace MarsCommerce.Core.Models
{
    public class Category : BaseEntity
    {
        public required string? Name { get; set; }
        public virtual List<Product>? Products { get; set; }
    }
}