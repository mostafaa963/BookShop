using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class AddCouponRequest
    {
        public string Code { get; set; } = string.Empty;
        public int DiscountValue { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int UsageLimit { get; set; }
    }
}
