using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public  interface ICategoryService
    {
        Task<CategoryIndexDto> GetCountAllCategoryAsync();
    }
}
