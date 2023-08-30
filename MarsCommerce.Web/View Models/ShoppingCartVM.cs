using MarsCommerce.Core.Models;

namespace MarsCommerce.Web.View_Models
{
    public class ShoppingCartVM
    {
        public int UserId { get; set; }
        public virtual UserVM? User { get; set; }
        public virtual List<ShoppingCartItemVM>? Items { get; set; }
    }
}
