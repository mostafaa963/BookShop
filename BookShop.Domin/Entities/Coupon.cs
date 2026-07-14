using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int DiscountValue { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int UsageLimit { get; set; }
        public int CountUsed { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<CouponUsage>? CouponUsages { get; set; }

    }
}
