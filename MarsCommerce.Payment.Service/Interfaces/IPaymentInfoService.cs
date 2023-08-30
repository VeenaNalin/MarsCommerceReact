using MarsCommerce.Core.Models;

namespace MarsCommerce.Payment.Service.Interfaces
{
    public interface IPaymentInfoService
    {

        Task<List<PaymentInfo>> GetPaymentInfo();
       
    }
}


