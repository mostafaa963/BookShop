using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class CategoryIndexDto
    {
        public List<GroupByDto>? Groups { get; set; }

        public int CountCategory { get; set; }

    }
}
