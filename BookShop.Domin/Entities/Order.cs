using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace BookShop.Domain.Entities
{
    //    UserId
    ////CreatedAt
    ////Status
    ////SubTotal
    ////Discount
    //TotalPrice
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Processing = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6

    }

    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal SubTotal { get; set; }
        public string? CouponCode { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem>? orderItems { get; set; }
    }
}
