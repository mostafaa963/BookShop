using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public  interface IFavoriteItemService
    {
        Task<FavoriteItemResponse> AddAsync(RequestFavoriteItemDto requestFavoriteItemDto, string UserId);
        Task<AllFavoriteItemResponse> GetAllFavoriteItemAsync(FavoriteQueryParameters favoriteQueryParameters, string userId);
        Task DeleteAsync(int bookId, string userId);
    }
}
