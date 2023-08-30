namespace MarsCommerce.Web.View_Models
{
    public class UserVM : BaseEntityVM
    {
        public UserVM()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid AzureUserId { get; set; }
        public string AuthToken { get; set; }
        public string UserRole { get; set; }
        public virtual ShoppingCartVM? Cart { get; set; }
        public virtual List<AddressVM>? Addresses { get; set; }
    }
}