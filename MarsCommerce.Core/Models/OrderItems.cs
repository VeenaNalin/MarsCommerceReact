using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class OrderItems : BaseEntity
    {
        public required int OrderId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public required decimal TotalPrice { get; set; }
        public virtual Product? Product { get; set; }

        public virtual Order? Order { get; set; }


        public static implicit operator List<object>(OrderItems? v)
        {
            throw new NotImplementedException();
        }

    }
}
