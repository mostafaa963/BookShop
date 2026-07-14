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
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<GenResponse<CartResponse>>> GetCartItems()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            var response = await _cartService.GetCartItems(userId);
            return Ok(new GenResponse<CartResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "CartItem Retrieved Successfully",
                Data = response
            });
        }
        [HttpPost("items")]
        public async Task<ActionResult<GenResponse<object>>> AddItem(AddCartItemRequest dto)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _cartService.AddToCart(dto, userId);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Item added to cart successfully.",
            });
        }
        [HttpGet("Items/InCerement")]
        public async Task<ActionResult<GenResponse<object>>> InCerement(int itemId)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _cartService.InCerement(itemId, userId);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Add item to Cart Successfully",
            });
        }
        [HttpGet("Items/DeCerement")]
        public async Task<ActionResult<GenResponse<object>>> DeCerement(int itemId)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _cartService.DeCerement(itemId, userId);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Add item to Cart Successfully",
            });
        }
        [HttpDelete("Items")]
        public async Task<ActionResult<GenResponse<object>>> RemoveItem(int itemId)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _cartService.RemoveCartItem(itemId, userId);

            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Remove CartITem  Successfully",
            });
        }
        [HttpDelete]
        public async Task<ActionResult<GenResponse<object>>> RemoveCart()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _cartService.RemoveCart(userId);

            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Cart is Deleted Successfully",
            });
        }
    }
}
