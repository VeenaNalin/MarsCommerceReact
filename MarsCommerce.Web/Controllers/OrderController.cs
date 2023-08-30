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
    public class OrderController : MarsCommerceBaseController
    {
        const string scopeRequiredByAPI = "demo.read";
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderController> _logger;
        public OrderController(HttpService httpService, ILogger<OrderController> logger, IConfiguration configuration) : base(httpService, configuration.GetSection("OrderServiceSettings:BaseUrl").Value ?? string.Empty)
        {
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("AddOrder")]
        public async Task<ActionResult<Order?>> AddOrder(Order order)
        {
            try
            {

                var response = await _httpService.HttpPostAsync<Order>(_configuration.GetValue<string>("OrderServiceSettings:AddOrder_Uri"),order);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "Order API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get Order details failed");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Order Details - BFF");
            }
            return NotFound();
        }


        [HttpGet("GetOrder/{id}")]
        public async Task<ActionResult<OrderVM?>> Get(int id)
        {
            try
            {
                var response = await _httpService.HttpGetAsync<OrderVM>(_configuration.GetValue<string>("OrderServiceSettings:GetOrderDetail_Uri") + Convert.ToString(id));
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);

                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "order API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get order details failed", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get order Details - BFF");
            }
            return NotFound();
        }
        [HttpGet("GetAllUserOrder/{userId}")]
        public async Task<ActionResult<OrderVM?>> GetAllUserOrder(int userId)
        {
            try
            {

                var response = await _httpService.HttpGetAsync<List<OrderVM>>(_configuration.GetValue<string>("OrderServiceSettings:GetOrder_Uri") + userId);
                if (response != null)
                {
                    if (response.Code == System.Net.HttpStatusCode.OK)
                        return Ok(response.Data);
                    else if (response.Code == System.Net.HttpStatusCode.InternalServerError)
                        return StatusCode(StatusCodes.Status500InternalServerError, "order get all API Returned an error!");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Web api call to get all product  failed", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - BFF");
            }
            return NotFound();
        }

    }
}
