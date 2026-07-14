using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.DAL;
using BockShop.DAL.Context.Identity;
using BockShop.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BockShop.BLL.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _unitOfWork = unitOfWork;
        }
        public async Task<AccessTokenDto> GenerateAccessTokenAsync(ApplicationUser User)
        {
            var roles = await _userManager.GetRolesAsync(User);

            var claims = new List<Claim> {
                new Claim (JwtRegisteredClaimNames.Sub,User.Id),
                new Claim (ClaimTypes.Name,User.UserName!),
                new Claim(ClaimTypes.Email,User.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)), SecurityAlgorithms.HmacSha256),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes)
                );

            return new AccessTokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken),
                ExpiredAccessTokenOn = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes)
            };
        }
        private string GenerateRefreshToken()
        {
            var token = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(token);
        }
        public async Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            var getRefreshToken = await _unitOfWork.RefreshToken.GetFirstOneAsync(e =>
            e.UserId == user.Id && e.ExpiredOn >= DateTime.Now && e.Revoked == null);
            if (getRefreshToken == null)
            {
                var refreshToken = new RefreshToken
                {
                    Token = GenerateRefreshToken(),
                    ExpiredOn = DateTime.UtcNow.AddDays(14),
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id,
                };
                await _unitOfWork.RefreshToken.AddAsync(refreshToken);
                await _unitOfWork.SaveChangeAsync();
                return refreshToken;
            }
            return getRefreshToken;
        }
    }
}
