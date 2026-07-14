using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Request
{
    public class FavoriteQueryParameters
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? Search { get; set; }
        public string? SortedBy { get; set; }
        public bool Descending { get; set; }
    }
}
