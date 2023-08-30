using FluentValidation;
using MarsCommerce.Core.Models;
using MarsCommerce.UserProfile.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarsCommerce.UserProfile.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : Controller
    {

        private readonly IUserProfileService _userprofileService;
        private readonly ILogger<UserProfileController> _logger;
        private readonly IValidator<User> _validatorUser;
        public UserProfileController(IUserProfileService userprofileService, IUserProfileService userprofileServiceNew, ILogger<UserProfileController> logger, IValidator<User> validatorUser)
        {
            _userprofileService = userprofileService;
            _logger = logger;
            _validatorUser = validatorUser;
        }
        [HttpGet]
        [Route("getaddress/{Id}")]
        public async Task<ActionResult<List<Address?>>> GetAddress(int Id)
        {
            try
            {
                return await _userprofileService.GetAddressAsync(Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get Address attributes - UserProfileController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Address attributes - UserProfileController");
            }
        }

        [HttpPost]
        [Route("addAddress")]
        public async Task<ActionResult<Address>> AddAddress(Address address)
        {
            try
            {
                return await _userprofileService.AddAddress(address);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Add Address - UserProfileController", address);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Address - UserProfileController");
            }
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            try
            {
                var modelValidate = _validatorUser.Validate(user);
                if (modelValidate.IsValid)
                {
                    var existingUser = _userprofileService.GetUserByAzureId(user.AzureUserId).Result;
                    if (existingUser != null)
                    {
                        existingUser.UserRole = user.UserRole;
                        existingUser.AuthToken = user.AuthToken;
                        existingUser.FirstName = user.FirstName;
                        existingUser.LastName = user.LastName;
                    }
                    else { existingUser = user; }
                    return await _userprofileService.RegisterUser(existingUser);
                }
                else
                {
                    var response = new
                    {
                        Message = "Bad Request - Model not valid",
                        Errors = modelValidate.Errors
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Register User - UserProfileController", user);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Register User - UserProfileController");
            }
        }

        [HttpPost]
        [Route("updatedefaultaddress")]
        public async Task<ActionResult<List<Address?>>> Updatedefaultaddress(Address address)
        {
            try
            {
                return await _userprofileService.Updatedefaultaddress(address.UserId, address.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update default address - UserProfileController", address.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update default address - UserProfileController");
            }
        }

    }
}
