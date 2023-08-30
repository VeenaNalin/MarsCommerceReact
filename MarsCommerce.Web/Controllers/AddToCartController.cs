using Ardalis.ApiClient;
using Azure;
using MarsCommerce.Web.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Identity.Web.Resource;

namespace MarsCommerce.Web.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [RequiredScope(scopeRequiredByAPI)]
    public class AddToCartController : MarsCommerceBaseController
    {
        const string scopeRequiredByAPI = "demo.read";
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public AddToCartController(HttpService httpService,ILogger<AddToCartController>logger,IConfiguration configuration) 
                                   :base(httpService, configuration.GetSection("ShoppingCartServiceSettings:BaseUrl").Value??string.Empty)
        {
            _configuration=configuration;
            _logger=logger;
        }


        [HttpPost("Cart")]      
        public async Task<ActionResult<ShoppingCartVM>> InsertItemToCart(ShoppingCartVM shoppingcart)
        {
            try
            {
                var response = await _httpService.HttpPostAsync<ShoppingCartVM>(Constants.ShoppingCartItem_Uri,shoppingcart);
                if(response != null) 
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)                 
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "ShoppingCart API Returned an error!");                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "web api call to insert product into cart failed", shoppingcart.UserId);
                return StatusCode(StatusCodes.Status500InternalServerError,"Failed to insert porduct into cart");
            }

            return NotFound();
        }
    }
}
