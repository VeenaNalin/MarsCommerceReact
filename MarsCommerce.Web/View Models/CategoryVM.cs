using MarsCommerce.Core.Models;

namespace MarsCommerce.Web.View_Models
{
    public class CategoryVM : BaseEntity
    {
        public required string? Name { get; set; }
        public virtual List<Product>? Products { get; set; }
    }
}
