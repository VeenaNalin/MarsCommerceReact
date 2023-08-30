using MarsCommerce.Core.Models;

namespace MarsCommerce.Web.View_Models
{
    public class ShoppingCartItemVM:BaseEntity
    {
        public int CartId { get; set; }
        public int ProductID { get; set; }
        public int Quntity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual ShoppingCartVM? ShoppingCart { get; set; }
        public virtual ProductVM? Product { get; set; }
    }
}
