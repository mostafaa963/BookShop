using BockShop.BLL.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderRequest dto, string userId);
    }
}
