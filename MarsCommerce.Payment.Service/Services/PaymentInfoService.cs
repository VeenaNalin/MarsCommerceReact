using MarsCommerce.Payment.Service.Interfaces;
using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;


namespace MarsCommerce.Payment.Service
{
    public class PaymentInfoService : IPaymentInfoService
    {
        private readonly IRepository<PaymentInfo> _PaymentInfoRepository;
        public PaymentInfoService(IRepository<PaymentInfo> PaymentInfoRepository)
        {
            _PaymentInfoRepository = PaymentInfoRepository;
        }
        public async Task<List<PaymentInfo>> GetPaymentInfo()
        {
            //throw new NotImplementedException();
            return await _PaymentInfoRepository.GetAllByAsync(c=>c.Id>0);
        }
    }
}
