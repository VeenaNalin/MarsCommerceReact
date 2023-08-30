using MarsCommerce.OrderManagement.Service.Interfaces;
using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using System.Reflection;
//using MarsCommerce.Infrastructure.Repository.Migrations;

namespace MarsCommerce.OrderManagement.Service
{
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IRepository<Order> _orderRepository;

        private readonly IRepository<OrderItems> _orderItemsRepository;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<PaymentInfo> _paymentRepository;
        public OrderManagementService(IRepository<Order> orderRepository, IRepository<OrderItems> orderItemsRepository, IRepository<ShoppingCartItem> shoppingCartItemRepository,
            IRepository<Product> productRepository, IRepository<PaymentInfo> paymentRepository)
        {
            _orderRepository = orderRepository;
            _orderItemsRepository = orderItemsRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
        }
        public async Task<Order> AddOrder(Order order)
        {
            Order orderResult = await _orderRepository.AddAsync(order);
            var shoppingCartId = _shoppingCartItemRepository.GetAllByAsync(x => x.ShoppingCart.UserId == order.UserId).Result.FirstOrDefault().CartId;
            List<ShoppingCartItem> cartItem = _shoppingCartItemRepository.GetAllByAsync(x => x.CartId == shoppingCartId).Result.ToList();
            foreach (var item in cartItem)
            {
                await _shoppingCartItemRepository.DeleteCartAsync(item.Id);
            }

            return orderResult;
        }
        public async Task<Order?> GetOrderDetailsAsync(int orderId)
        {
            List<OrderItems> listItams = new List<OrderItems>();
            var ss = await _orderRepository.GetAsync(orderId);
            listItams = await _orderItemsRepository.GetAllByAsync(x => x.OrderId == orderId);
            foreach (var item in listItams)
            {
                item.Product = await _productRepository.GetAsync(x => x.Id == item.ProductId);
            }

            ss.PaymentInfo = await _paymentRepository.GetAsync(ss.PaymentInfoId);
            ss.Items = listItams;

            return ss;

        }

        public async Task<List<Order?>> GetOrderDetailsByUserIdAsync(int userId)
        {

            List<OrderItems> listItams = new List<OrderItems>();
            var ss = _orderRepository.GetAllByAsync(x => x.UserId == userId).Result.OrderByDescending(x => x.Id).ToList();
            int i = 0;
            foreach (var items in ss)
            {
                listItams = await _orderItemsRepository.GetAllByAsync(x => x.OrderId == items.Id);
                items.Items = listItams;
                items.PaymentInfo = await _paymentRepository.GetAsync(items.PaymentInfoId);
            }


            return ss;
        }

    }

}
