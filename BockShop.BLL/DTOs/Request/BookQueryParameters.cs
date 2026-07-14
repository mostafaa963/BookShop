using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class BookQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool DescendingOrder { get; set; }

    }
}
