using MarsCommerce.Core.Models;

namespace MarsCommerce.Web.View_Models
{
    public class OrderItems : BaseEntity
    {
        public required int OrderId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
        public virtual Product? Product { get; set; }
    }
}
