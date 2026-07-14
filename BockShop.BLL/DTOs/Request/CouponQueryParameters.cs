using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class CouponQueryParameters
    {
        public  int PageNumber { get; set; }
        public  int PageSize { get; set; }

        public string? Search { get; set; }
        public string? SortedBy { get; set; }
        public bool Descending { get; set; }

    }
}
