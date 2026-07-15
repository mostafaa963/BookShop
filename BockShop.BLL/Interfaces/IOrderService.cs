using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderRequest dto, string userId);
        Task UpdateOrderStatus(UpdateOrderStatusRequest dto);
        Task CancelOrder(int orderId, string userId);
        Task<OrderDetailsResponse> GetOrderById(int Id, string userId);
        Task<IEnumerable<OrdersResponse>> GetAllOrdersAsync(OrderQueryParameters dto, string userId);
    }
}
