using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class ShoppingCartItem : BaseEntity
    { 
        public int CartId { get; set; }
        public int ProductID { get; set; }
        public int Quntity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }
        public virtual Product? Product { get; set; }

    }
}
