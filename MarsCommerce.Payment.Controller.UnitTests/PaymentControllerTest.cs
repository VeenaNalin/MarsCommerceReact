


using MarsCommerce.Payment.Service;
using Microsoft.Extensions.Logging;
using MarsCommerce.Payment.Service.Controllers;
using MarsCommerce.Payment.Service.Interfaces;
using Moq;
using System.Linq.Expressions;
using Xunit;
using MarsCommerce.Core.Models;

namespace MarsCommerce.Payment.Controller.UnitTests
{
    public class PaymentControllerTest
    {
        private readonly Mock<IPaymentInfoService> _paymentService;
        private readonly Mock<ILogger<PaymentController>> _logger;
        private readonly PaymentController paymentController;
        public PaymentControllerTest()
        {
            _paymentService = new Mock<IPaymentInfoService>();
            _logger = new Mock<ILogger<PaymentController>>();
            paymentController = new PaymentController(_paymentService.Object, _logger.Object);
        }
        [Fact]
        public void Get_PaymentDetails_Success()
        {
            // Arrange            
            List<PaymentInfo> listPayment = new List<PaymentInfo>();
            PaymentInfo payment=new PaymentInfo
            {
                PaymentMethod= "Cash On Delivery"
            };
            listPayment.Add(payment);
            _paymentService.Setup(x => x.GetPaymentInfo()).Returns(Task.Run(() => listPayment));
            var result = paymentController.GetPaymentInfo().Result;

            Assert.True(result.Count > 0);
        }
    }
}