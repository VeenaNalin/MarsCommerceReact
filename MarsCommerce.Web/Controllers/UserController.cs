using Ardalis.ApiClient;
using FluentValidation;
using MarsCommerce.Core.Models;
using MarsCommerce.Web.View_Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MarsCommerce.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : MarsCommerceBaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IValidator<UserVM> _validatorUser;

        public UserController(HttpService httpService, ILogger<UserController> logger, IConfiguration configuration, IValidator<UserVM> validatorUser) : base(httpService, configuration.GetSection("UserProfileServiceSettings:BaseUrl").Value ?? string.Empty)
        {
            _configuration = configuration;
            _logger = logger;
            _validatorUser = validatorUser;
        }
        [HttpGet("ListAddress/{Id}")]
        public async Task<ActionResult<List<AddressVM?>>> ListAddress(int Id)
        {
            try
            {

                var response = await _httpService.HttpGetAsync<List<AddressVM>>(Constants.ListAddress_Uri + Convert.ToString(Id));
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "UserProfile API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to List Address details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to List Address details - BFF");
            }
            return NotFound();
        }


        [HttpPost]
        [Route("addAddress")]
        public async Task<ActionResult<AddressVM?>> AddAddress(Address address)
        {
            try
            {
                var response = await _httpService.HttpPostAsync<Address>(_configuration.GetValue<string>("UserProfileServiceSettings:AddAddress_Uri"), address);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "User API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to Save Address details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Save Address details - BFF");
            }
            return NotFound();
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<ActionResult<UserVM?>> RegisterUser(UserVM user)
        {
            try
            {
                var modelValidate = _validatorUser.Validate(user);
                if (modelValidate.IsValid)
                {
                    var response = await _httpService.HttpPostAsync<UserVM>(_configuration.GetValue<string>("UserProfileServiceSettings:RegisterUser_Uri"), user);
                    if (response != null)
                    {
                        if (response.Code == System.Net.HttpStatusCode.OK)
                            return Ok(response.Data);
                        else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                            return StatusCode(StatusCodes.Status500InternalServerError, "User API Returned an error!");
                    }
                    return NotFound();
                }
                else
                {
                    var response = new { Message = "Bad Request - Model not valid", Errors = modelValidate.Errors };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to Register User failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Register the user - BFF");
            }
        }

        [HttpPost]
        [Route("UpdateDefaultAddress")]
        public async Task<ActionResult<List<AddressVM?>>> UpdateDefaultAddress([FromBody] dynamic jsonData)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonData.ToString());
                int userId = data.userId;
                int addressId = data.addressId;


                if (userId > 0)
                {
                    var addressData = new Address { UserId = userId, Id = addressId, Mobile = 0, Address1 = "", Address2 = "", City = "", PinCode = 0, State = "", User = null };
                    var response = await _httpService.HttpPostAsync<List<Address>>(_configuration.GetValue<string>("UserProfileServiceSettings:UpdateDefaultAddress_Uri"), addressData);
                    if (response != null)
                    {
                        if (response.Code == System.Net.HttpStatusCode.OK)
                            return Ok(response.Data);
                        else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                            return StatusCode(StatusCodes.Status500InternalServerError, "User API Returned an error!");
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to UpdateDefault Address details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to UpdateDefault Address details - BFF");
            }
            return NotFound();
        }
    }

}