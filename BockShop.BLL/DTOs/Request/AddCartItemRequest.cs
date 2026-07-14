using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class AddCartItemRequest
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
