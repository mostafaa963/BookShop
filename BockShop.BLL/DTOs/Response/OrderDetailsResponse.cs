using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class OrderDetailsResponse
    {
        public IEnumerable<OrderItemResponse>? Items { get; set; }
        public string? CouponCode { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
