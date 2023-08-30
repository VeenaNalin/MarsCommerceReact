using Ardalis.ApiClient;
using MarsCommerce.Core.Models;
using MarsCommerce.Web.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace MarsCommerce.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [RequiredScope(scopeRequiredByAPI)]
    public class PaymentController : MarsCommerceBaseController
    {
        const string scopeRequiredByAPI = "demo.read";
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(HttpService httpService, ILogger<PaymentController> logger, IConfiguration configuration) : base(httpService, configuration.GetSection("PaymentServiceSettings:BaseUrl").Value ?? string.Empty)
        {
            _configuration = configuration;
            _logger = logger;
        }


        [HttpGet("payment")]
        public async Task<ActionResult<List<PaymentInfo>?>> GetPayment()
        {
            try
            {
               
                var response = await _httpService.HttpGetAsync<List<PaymentInfo>>(_configuration.GetValue<string>("PaymentServiceSettings:Payment_Uri"));
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Catalog API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get product details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - BFF");
            }
            return NotFound();
        }

    }
}
