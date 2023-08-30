using Ardalis.ApiClient;
using MarsCommerce.Web.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace MarsCommerce.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [RequiredScope(scopeRequiredByAPI)]
    public class ShoppingCartController : MarsCommerceBaseController
    {
        const string scopeRequiredByAPI = "demo.read";
        private readonly IConfiguration _configuration;
        private readonly ILogger<ShoppingCartController> _logger;
        public ShoppingCartController(HttpService httpService, ILogger<ShoppingCartController> logger, IConfiguration configuration) : base(httpService, configuration.GetSection("ShoppingCartServiceSettings:BaseUrl").Value ?? string.Empty)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("GetCartItem/{id}")]
        public async Task<ActionResult<ShoppingCartVM?>> GetCartItem(int id)
        {
            try
            {
                var response = await _httpService.HttpGetAsync<ShoppingCartVM>(Constants.ShoppingCart_Uri + Convert.ToString(id));
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);

                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "ShoppingCart API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Web api call to get Cart details failed", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Cart Details - BFF");
            }
        }

        [HttpPost("UpdateCount")]
        public async Task<ActionResult<ShoppingCartItemVM?>> UpdateCount(ShoppingCartItemVM shoppingCartItem)
        {
            try
            {
               var response = await _httpService.HttpPostAsync<ShoppingCartItemVM>(Constants.ShoppingCartUpdate_Uri , shoppingCartItem);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);

                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "ShoppingCartItem API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get Cart details failed", shoppingCartItem);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get ShoppingCartItem Details - BFF");
            }
        }
        [HttpPost("RemoveProductFromCart")]
        public async Task<ActionResult<ShoppingCartItemVM?>> RemoveProductFromCart(ShoppingCartItemVM shoppingCartItem)
        {
            try
            {
                var response = await _httpService.HttpPostAsync<ShoppingCartItemVM>(Constants.ShoppingCartRemove_Uri , shoppingCartItem);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);

                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "ShoppingCartItem API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get Cart details failed", shoppingCartItem);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get ShoppingCartItem Details - BFF");
            }
        }
    }
}
