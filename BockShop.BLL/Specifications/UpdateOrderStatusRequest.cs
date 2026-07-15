using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
