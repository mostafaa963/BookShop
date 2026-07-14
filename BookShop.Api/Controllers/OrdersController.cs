using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<GenResponse<object>>> CreateOrder(CreateOrderRequest dto)
        {
            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            await _orderService.CreateOrderAsync(dto, userId);
           return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode=200,
                Message="Create Order Successfully",
            });
    }
    }
}
