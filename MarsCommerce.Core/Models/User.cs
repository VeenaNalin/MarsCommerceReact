namespace MarsCommerce.Core.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid AzureUserId { get; set; }
        public string AuthToken { get; set; }
        public string UserRole { get; set; }
        public virtual ShoppingCart? Cart { get; set; }
        public virtual List<Address>? Addresses { get; set; }
        public virtual List<Order>? Orders { get; set; }

    }
}
