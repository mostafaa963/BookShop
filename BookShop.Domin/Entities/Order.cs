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
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal SubTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem>? orderItems { get; set; }
    }
}
