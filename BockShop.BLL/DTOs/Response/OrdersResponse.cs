using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class OrdersResponse
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus status { get; set; }
    }
}
