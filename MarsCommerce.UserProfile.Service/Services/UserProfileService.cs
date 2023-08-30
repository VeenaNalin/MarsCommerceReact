using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using MarsCommerce.UserProfile.Service.Interfaces;

namespace MarsCommerce.UserProfile.Service
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<User> _userRepository;

        public UserProfileService(IRepository<Address> addressRepository, IRepository<User> userRepository)
        {

            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }
        public async Task<Address> AddAddress(Address address)
        {
            return await _addressRepository.AddAsync(address);

        }

        public async Task<User> RegisterUser(User user)
        {
            if (user.Id > 0)
            {
                return await _userRepository.UpdateAsync(user);
            }
            else
            {
                return await _userRepository.AddAsync(user);
            }
        }
        
        public async Task<User> GetUserByAzureId(Guid azureUserId)
        {
            try
            {
                return _userRepository.GetAllByAsync(x => x.AzureUserId == azureUserId).Result.FirstOrDefault();

            }
            catch (Exception ex) {
                return null;
            }            
        }

        public async Task<List<Address?>> GetAddressAsync(int userId)
        {
            return await _addressRepository.GetAllByAsync(x => x.UserId == userId);
        }
        public async Task<List<Address>> Updatedefaultaddress(int userId, int addressId)
        {
            List<Address> listAddress = _addressRepository.GetAllByAsync(c => c.UserId == userId).Result.ToList();
            if (listAddress != null)
            {
                for (int i = 0; i < listAddress.Count; i++)
                {
                    if (listAddress[i].Id == addressId)
                    {
                        listAddress[i].IsDefaultAddress = true;
                    }
                    else
                    {
                        listAddress[i].IsDefaultAddress = false;
                    }
                    await _addressRepository.UpdateAsync(listAddress[i]);
                }
            }
            return _addressRepository.GetAllByAsync(c => c.UserId == userId).Result.ToList();
        }
    }
}

