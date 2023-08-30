using MarsCommerce.Core.Models;

namespace MarsCommerce.UserProfile.Service.Interfaces
{
    public interface IUserProfileService
    {
        Task<Address> AddAddress(Address address);
        Task<User> RegisterUser(User user);
        Task<List<Address?>> GetAddressAsync(int userId);
        Task<List<Address?>> Updatedefaultaddress(int userId, int addressId);
        Task<User> GetUserByAzureId(Guid azureUserId);
    }
}
