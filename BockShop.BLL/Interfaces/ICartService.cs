using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> GetCartItems(string userId);

        Task DeCerement(int itemId, string userId);
        Task AddToCart(AddCartItemRequest dto, string userId);
        Task InCerement(int itemId, string userId);
        Task RemoveCart(string userId);
        Task RemoveCartItem(int itemId, string userId);
    }
}
