namespace MarsCommerce.Core.Models
{
    public class ProductAttributeMapping : BaseEntity
    {
        public ProductAttributeMapping() {
            ProductAttributeValue = string.Empty;        
        }
        public int ProductId { get; set; }
        public int ProductAttributeId { get; set; }
        public string ProductAttributeValue { get; set; }

        public virtual Product? Product { get; set; }
        public virtual ProductAttribute? ProductAttribute { get; set; }
    }
}