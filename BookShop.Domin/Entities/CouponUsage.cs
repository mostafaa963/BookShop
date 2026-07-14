using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class CouponUsage
    {
        public int Id { get; set; }
        public  int CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public string Code { get; set; }
        public string userId { get; set; }
        public DateTime UsedAt { get; set; }
    }
}
