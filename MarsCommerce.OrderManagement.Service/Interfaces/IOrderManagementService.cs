using MarsCommerce.Core.Models;

namespace MarsCommerce.OrderManagement.Service.Interfaces
{
    public interface IOrderManagementService
    {
        Task<Order> AddOrder(Order order);
        Task<Order?> GetOrderDetailsAsync(int orderId);


        Task<List<Order?>> GetOrderDetailsByUserIdAsync(int userId);
    }
}