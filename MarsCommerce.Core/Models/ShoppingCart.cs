using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class ShoppingCart : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual List<ShoppingCartItem>? Items { get; set; }

          

    }
}
