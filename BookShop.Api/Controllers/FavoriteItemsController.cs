using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteItemsController : ControllerBase
    {
        private readonly IFavoriteItemService _favoriteItemService;

        public FavoriteItemsController(IFavoriteItemService favoriteItemService)
        {
            _favoriteItemService = favoriteItemService;
        }

        [HttpGet]
        public async Task<ActionResult<GenResponse<AllFavoriteItemResponse>>> Favorites([FromQuery]FavoriteQueryParameters dto)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            var response = await _favoriteItemService.GetAllFavoriteItemAsync(dto, userId);

            return Ok(new GenResponse<AllFavoriteItemResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "Favorite items retrieved successfully.",
                Data = response
            });
        }

        [HttpPost]
        public async Task<ActionResult<GenResponse<object>>> AddFavoriteItem(RequestFavoriteItemDto requestFavoriteItemDto)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var response = await _favoriteItemService.AddAsync(requestFavoriteItemDto, userId!);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Book added to favorites successfully",
                Data = response
            });
        }

        [HttpDelete]
        public async Task<ActionResult<GenResponse<object>>> Delete(int bookId)
        {
            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _favoriteItemService.DeleteAsync(bookId, userId);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Favorite Item Deleted Successfully",
            });
        }
    }
}
