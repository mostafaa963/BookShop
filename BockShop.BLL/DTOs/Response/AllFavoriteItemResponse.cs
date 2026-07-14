using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.DTOs.Response
{
    public class AllFavoriteItemResponse
    {
       public IEnumerable<FavoriteItemResponse>? Data { get; set; }

    }
}
