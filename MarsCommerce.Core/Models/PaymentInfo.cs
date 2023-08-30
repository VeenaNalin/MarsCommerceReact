using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class PaymentInfo : BaseEntity
    {
        public required string PaymentMethod { get; set; }
    }
}
