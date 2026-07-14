using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public  interface IAuthService
    {
        Task<ResponseRegisterDto> AddUserAsync(RequestRegisterUserDto requestRegisterUserDto);
        Task<ResponseLoginDto> LoginAsync(RequestLoginDto requestLoginDto);
        Task<ResponseRefreshTokenDto> CreateRefreshTokenAsync(string? refreshToken);
        Task LogOutAsync(string? refreshToken, string userId);
        Task ChangePasswordAsync(RequestChangePasswordDto requestChangePasswordDto);
        Task<string> ForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto);
        Task ResetPasswordAsync(ResetPasswordRequestDto request);
        Task ConfirmEmailAsync(string token, string userId);
        Task<EmailConfirmationTokenResponse> GenerateTokensForConfirmEmail(string email);
    }
}
