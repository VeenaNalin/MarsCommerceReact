using MarsCommerce.OrderManagement.Service.Interfaces;
using MarsCommerce.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace MarsCommerce.OrderManagement.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderManagementController : Controller
    {

        private readonly IOrderManagementService _OrderManagementService;
        private readonly ILogger<OrderManagementController> _logger;
        public OrderManagementController(IOrderManagementService OrderManagementService, ILogger<OrderManagementController> logger)
        {
            _OrderManagementService = OrderManagementService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<ActionResult<Order?>> AddOrder(Order order)
        {
            try
            {
                return await _OrderManagementService.AddOrder(order);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get order Details - orderController");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Adda a new order - OrderController");
               
            }
            // 
        }
        [HttpGet]
        [Route("order/{Id}")]
        public async Task<ActionResult<Order?>> GetOrder(int Id)
        {
            try
            {
                Order? order = await _OrderManagementService.GetOrderDetailsAsync(Id);

                return order;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Product Details - CatalogController", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - CatalogController");
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetOrderByUserid/{UserId}")]
        public async Task<ActionResult<List<Order?>>> GetOrderByUserId(int UserId)
        {
            try
            {
                List<Order?> order = new List<Order?>();
                return order = await _OrderManagementService.GetOrderDetailsByUserIdAsync(UserId);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Product Details - CatalogController", UserId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - CatalogController");
            }
            return NoContent();
        }
    }
}
