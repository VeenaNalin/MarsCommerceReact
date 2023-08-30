using MarsCommerce.Core.Models;

namespace MarsCommerce.Web.View_Models
{
    public class AddressVM : BaseEntity
    {
        public int UserId { get; set; }
        public required long Mobile { get; set; }
        public required string Address1 { get; set; }
        public required string Address2 { get; set; }
        public required string City { get; set; }
        public required int PinCode { get; set; }
        public required string State { get; set; }
        public bool IsDefaultAddress { get; set; }
        public virtual User? User { get; set; }

    }
}
