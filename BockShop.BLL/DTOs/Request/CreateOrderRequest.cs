using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class CreateOrderRequest
    {
        public string? CouponCode { get; set; }
    }
}
