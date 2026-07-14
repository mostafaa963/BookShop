using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.DAL.Context.Identity;
using BockShop.DAL.Interfaces;
using BockShop.DAL.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BockShop.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IValidator<RequestRegisterUserDto> _requestRegisterUserDtoValidator;
        private readonly IValidator<RequestForgetPasswordDto> _requestForgetPasswordDtoValidator;
        private readonly IValidator<RequestChangePasswordDto> _requestChangePasswordDtoValidator;
        private readonly IValidator<ResetPasswordRequestDto> _resetPasswordRequestValidator;
        private readonly IValidator<RequestLoginDto> _RequestLoginDtoValidator;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;


        public AuthService(IValidator<ResetPasswordRequestDto> resetPasswordRequestValidator, IValidator<RequestForgetPasswordDto> requestForgetPasswordDtoValidator, IValidator<RequestChangePasswordDto> requestChangePasswordDtoValidator, IUnitOfWork unitOfWork, ITokenService jwtTokenService, IValidator<RequestLoginDto> RequestLoginDtoValidator, IValidator<RequestRegisterUserDto> requestRegisterUserDtoValidator, UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _RequestLoginDtoValidator = RequestLoginDtoValidator;
            _requestRegisterUserDtoValidator = requestRegisterUserDtoValidator;
            _requestChangePasswordDtoValidator = requestChangePasswordDtoValidator;
            _userManger = userManger;
            _signInManager = signInManager;
            _requestForgetPasswordDtoValidator = requestForgetPasswordDtoValidator;
            _resetPasswordRequestValidator = resetPasswordRequestValidator;
        }

        public async Task<ResponseRegisterDto> AddUserAsync(RequestRegisterUserDto requestRegisterUserDto)
        {
            var validatorRegister = await _requestRegisterUserDtoValidator.ValidateAsync(requestRegisterUserDto);
            if (!validatorRegister.IsValid)
                throw new ValidationException(validatorRegister.Errors);

            var User = new ApplicationUser
            {
                UserName = requestRegisterUserDto.UserName,
                FullName = requestRegisterUserDto.FullName,
                Email = requestRegisterUserDto.Email,
            };
            var result = await _userManger.CreateAsync(User, requestRegisterUserDto.Password);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
            return new ResponseRegisterDto
            {
                UserId=User.Id,
                UserName = requestRegisterUserDto.UserName,
                Email = requestRegisterUserDto.Email,
            };

        }
        public async Task<ResponseLoginDto> LoginAsync(RequestLoginDto requestLoginDto)
        {
            var loginValidator = await _RequestLoginDtoValidator.ValidateAsync(requestLoginDto);
            if (!loginValidator.IsValid)
                throw new ValidationException(loginValidator.Errors);

            var user = await _userManger.FindByNameAsync(requestLoginDto.UserName);
            if (user == null)
                throw new Exception("In valid UserName And Password");
            var result = await _signInManager.CheckPasswordSignInAsync(user, requestLoginDto.Password, true);
            if (!result.Succeeded)
                throw new Exception("InValid UserName and Password");
            if (result.IsLockedOut)
                throw new Exception("You have been Blocked ");

            var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(user);

            return new ResponseLoginDto
            {
                RefreshToken = refreshToken.Token,
                ExpiredRefreshToken = refreshToken.ExpiredOn,
                AccessToken = accessToken.AccessToken,
                ExpiredAccessToken = accessToken.ExpiredAccessTokenOn,
                FullName = user.FullName
            };
        }
        public async Task LogOutAsync(string? refreshToken, string userId)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new Exception("InValid Token");
            var getRefreshToken = await _unitOfWork.RefreshToken.GetFirstOneAsync(e => e.Token == refreshToken
            && e.Revoked == null && e.ExpiredOn >= DateTime.UtcNow);
            if (getRefreshToken == null)
                throw new Exception("InValid Token");

            var allRefreshedToken = _unitOfWork.RefreshToken.GetAll();
            allRefreshedToken = allRefreshedToken.Where(e => e.UserId == userId && e.Revoked == null && e.ExpiredOn >= DateTime.UtcNow);
            foreach (var item in allRefreshedToken)
                item.Revoked = DateTime.UtcNow;
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task<ResponseRefreshTokenDto> CreateRefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new Exception("InValid Token");
            var getRefreshToken = await _unitOfWork.RefreshToken.GetFirstOneAsync(e => e.Token == refreshToken
            && e.Revoked == null && e.ExpiredOn >= DateTime.UtcNow);
            if (getRefreshToken == null)
                throw new Exception("InValid Token2");
            var user = await _userManger.FindByIdAsync(getRefreshToken.UserId);
            if (user == null)
                throw new Exception("InValid Token3");
            getRefreshToken.Revoked = DateTime.UtcNow;
            _unitOfWork.RefreshToken.Update(getRefreshToken);
            await _unitOfWork.SaveChangeAsync();
            var accessToken = await _jwtTokenService.GenerateAccessTokenAsync(user);
            var newRefreshToken = await _jwtTokenService.GenerateRefreshTokenAsync(user);

            return new ResponseRefreshTokenDto
            {
                RefreshToken = newRefreshToken.Token,
                ExpiredRefreshToken = newRefreshToken.ExpiredOn,
                AccessToken = accessToken.AccessToken,
                ExpiredAccessToken = accessToken.ExpiredAccessTokenOn,
            };
        }
        public async Task ChangePasswordAsync(RequestChangePasswordDto requestChangePasswordDto)
        {
            var validatorUserName = await _requestChangePasswordDtoValidator.ValidateAsync(requestChangePasswordDto);
            if (!validatorUserName.IsValid)
                throw new ValidationException(validatorUserName.Errors);
            var user = await _userManger.FindByEmailAsync(requestChangePasswordDto.EmailOrUserName)
                ?? await _userManger.FindByNameAsync(requestChangePasswordDto.EmailOrUserName);
            if (user is null)
                throw new Exception("InValid UserName Or Email");
            var result = await _userManger.ChangePasswordAsync(user, requestChangePasswordDto.Password, requestChangePasswordDto.NewPassword);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
            var getAllRefreshToken = _unitOfWork.RefreshToken.GetAll();

            var ALlRefreshToken = await getAllRefreshToken.Where(e => e.UserId == user.Id && e.Revoked == null && e.ExpiredOn >= DateTime.UtcNow).ToListAsync();
            foreach (var refreshToken in ALlRefreshToken)
                refreshToken.Revoked = DateTime.UtcNow;
        }
        public async Task<string> ForgetPasswordAsync(RequestForgetPasswordDto requestForgetPasswordDto)
        {
            var validatorForgetPassword = await _requestForgetPasswordDtoValidator.ValidateAsync(requestForgetPasswordDto);
            if (!validatorForgetPassword.IsValid)
                throw new ValidationException(validatorForgetPassword.Errors);
            var user = await _userManger.FindByEmailAsync(requestForgetPasswordDto.Email);
            if (user is null)
                throw new Exception("If your email exists, a reset link has been sent.");

            return await _userManger.GeneratePasswordResetTokenAsync(user);
        }
        public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var validation = await _resetPasswordRequestValidator.ValidateAsync(request);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var user = await _userManger.FindByEmailAsync(request.Email);
            if (user is null)
                throw new Exception("User Not Found");

            var resultResetPassword = await _userManger.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!resultResetPassword.Succeeded)
                throw new ValidationException(resultResetPassword.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));
            var refreshToken = _unitOfWork.RefreshToken.GetAll();
            refreshToken = refreshToken.Where(e => e.UserId == user.Id && e.Revoked == null && e.ExpiredOn >= DateTime.UtcNow);
            foreach (var item in refreshToken)
            {
                item.Revoked = DateTime.UtcNow;
            }
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task<EmailConfirmationTokenResponse> GenerateTokensForConfirmEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Email Is not Found");
            var user = await _userManger.FindByEmailAsync(email);
            if (user is null)
                throw new Exception("User Not Found");
            if (user.EmailConfirmed)
                throw new Exception("Your Email is Confirmed");
            var token = await _userManger.GenerateEmailConfirmationTokenAsync(user);
            return new EmailConfirmationTokenResponse
            {
                EmailConfirmationToken = token,
                userId = user.Id
            };
        }
        public async Task ConfirmEmailAsync(string token, string userId)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token Is null ");

            var user = await _userManger.FindByIdAsync(userId);
            if (user is null)
                throw new Exception("User is Not Found");
            var result = await _userManger.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));

        }
    }
}
