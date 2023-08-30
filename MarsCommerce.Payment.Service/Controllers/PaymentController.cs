using MarsCommerce.Payment.Service.Interfaces;
using MarsCommerce.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace MarsCommerce.Payment.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {

        private readonly IPaymentInfoService _PaymentInfoService;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentInfoService PaymentInfoService, ILogger<PaymentController> logger)
        {
            _PaymentInfoService = PaymentInfoService;
            _logger = logger;
        }

        [HttpGet]
        [Route("getpaymentinfo")]
        public async Task<List<PaymentInfo>>  GetPaymentInfo()
        {
            try
            {
                return await _PaymentInfoService.GetPaymentInfo();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Payment Details - PaymentController");
                return new List<PaymentInfo>();
                //retun StatusCode(StatusCodes.Status500InternalServerError, "Failed to Get Product Details - PaymentController");
            }
           // 
        }

    }
}
