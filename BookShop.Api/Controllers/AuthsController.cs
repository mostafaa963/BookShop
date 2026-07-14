using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthsController(IEmailService emailService, IAuthService authService)
        {
            _emailService = emailService;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GenResponse<ResponseRegisterDto>>> Register(RequestRegisterUserDto requestRegisterUserDto)
        {
            var response = await _authService.AddUserAsync(requestRegisterUserDto);
            var emailToken = await _authService.GenerateTokensForConfirmEmail(response.Email);
            var link  = Url.Action(nameof(AuthsController.Confirm),"Auths",new { emailToken.EmailConfirmationToken, emailToken.userId},Request.Scheme);
           await _emailService.SendEmailAsync( requestRegisterUserDto.Email,link!);
            return CreatedAtAction(nameof(Login), new GenResponse<ResponseRegisterDto>
            {
                Success = true,
                StatusCode = 201,
                Message = "Create Account Successfully..",
                Data = response
            });
        }
        [HttpPost("Login")]
        public async Task<ActionResult<GenResponse<ResponseLoginDto>>> Login(RequestLoginDto requestLoginDto)
        {
            var response = await _authService.LoginAsync(requestLoginDto);

            return Ok(new GenResponse<ResponseLoginDto>
            {
                Success = true,
                StatusCode = 200,
                Message = $"Welcome Back {response.FullName}",
                Data = response
            });
        }
        [Authorize]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<GenResponse<ResponseRefreshTokenDto>>> RefreshToken(string token)
        {
            var response = await _authService.CreateRefreshTokenAsync(token);
            return Ok(new GenResponse<ResponseRefreshTokenDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Refresh Token successfully",
                Data = response
            });
        }
        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<GenResponse<object>>> LogOut(string RefreshToken)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            await _authService.LogOutAsync(RefreshToken, userId!);

            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "LogOut Successfully.."
            });
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<GenResponse<object>>> ChangePassword(RequestChangePasswordDto requestChangePasswordDto)
        {

            await _authService.ChangePasswordAsync(requestChangePasswordDto);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Change Password Successfully..",
            });
        }
        // [Otp] this way batter in app ---------  [Identity to ResetPassword] is good in Web 
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<GenResponse<object>>> ForgetPassword(RequestForgetPasswordDto dto)
        {
            var Token = await _authService.ForgetPasswordAsync(dto);
            // frontend المفروض يعمل صفحه get 
            var link = Url.Action(nameof(ResetPassword), "Auths", new { Email = dto.Email, Token }, Request.Scheme);
            await _emailService.SendEmailAsync(dto.Email, link!, EmailType.ResetPassword);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "If your email exists, a reset link has been sent."
            });

        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<GenResponse<object>>> ResetPassword(ResetPasswordRequestDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "ResetPassword Successfully",
            });
        }
        [HttpGet("Confirm-Email")]
        public async Task<ActionResult<GenResponse<object>>> Confirm(string token, string userId)
        {
            await _authService.ConfirmEmailAsync(token, userId);

            return Ok(new GenResponse<object>
            {
                Success= true,
                StatusCode=200,
                Message="Confirm Email Successfully",
            });

        }
        [HttpPost("Reconfirm-Email")]
        public async Task<ActionResult<GenResponse<object>>> Reconfirm(ReconfirmEmailRequest reconfirmEmailRequest)
        {
            var confirmation = await _authService.GenerateTokensForConfirmEmail(reconfirmEmailRequest.Email);
            var link = Url.Action(nameof(AuthsController.Confirm), "Auths", new { confirmation.EmailConfirmationToken, confirmation.userId }, Request.Scheme);
           await _emailService.SendEmailAsync(reconfirmEmailRequest.Email, link);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode=200,
                Message= "Confirmation email has been sent."
            });

        }
    }
}
