using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class CouponResponse
    {
        public string code { get; set; }
        public int DiscountValue { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int UsageLimit { get; set; }
        public int CountUsed { get; set; }
        public bool IsActive { get; set; }

    }
}
