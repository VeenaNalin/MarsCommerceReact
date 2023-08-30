

using MarsCommerce.Core.Models;
using MarsCommerce.Payment.Service;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace MarsCommerce.Payment.Service.UnitTests
{
    public class PaymentServiceTest
    {
        private readonly Mock<IRepository<PaymentInfo>> _PaymentRepositoryMock;
        

        public PaymentServiceTest()
        {
            _PaymentRepositoryMock = new Mock<IRepository<PaymentInfo>>();            
        }
        [Fact]
        public void Get_PaymentDetails_Success()
        {
            // Arrange            
            List<PaymentInfo> Payment = new List<PaymentInfo>();
            Payment.Add(new PaymentInfo()
            {
                PaymentMethod="Cash On Delivery"
            });
            _PaymentRepositoryMock.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<PaymentInfo, bool>>>())).Returns(Task.Run(() => Payment));//.Returns(Task.Run(() =>Payment));
            var paymentService = new PaymentInfoService(_PaymentRepositoryMock.Object);


            // Act
            var result = paymentService.GetPaymentInfo().Result;
            // Assert
           

            Assert.True(result.Count > 0);


        }
    }
}