using BockShop.BLL.DTOs.Response;
using BockShop.DAL.Context.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface ITokenService
    {
         Task<AccessTokenDto> GenerateAccessTokenAsync(ApplicationUser User);
        Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user);
    }
}
