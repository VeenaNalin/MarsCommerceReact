using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class Order:BaseEntity
    {
        public required int UserId { get; set; }
        
        public required DateTime OrderDate { get; set; }

        public required int PaymentInfoId { get; set; }

        public Decimal DeliveryCharge { get; set; }

        public DateTime DeliveryDate { get; set; }

        public required int AddressId { get; set; }

        public required decimal OrderTotal { get; set; }
        public virtual PaymentInfo? PaymentInfo { get; set; }

        public virtual Address? Address { get; set; }

        public virtual User? User { get; set; }
        public virtual List<OrderItems>? Items { get; set; }

        
    }
}
