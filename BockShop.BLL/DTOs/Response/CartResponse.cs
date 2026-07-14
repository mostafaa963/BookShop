using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class CartResponse
    {
        public IEnumerable<CartItemResponse> cartItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalPrice { get; set; }
        public int TotalItems { get; set; }
    }
}
