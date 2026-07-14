using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public  class CartItemResponse
    {
        public string BookTitle { get; set; }
        public string ASIN { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice => UnitPrice * Quantity;
        public string  AuthorName { get; set; }
    }
}
