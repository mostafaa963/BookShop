using Azure;
using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using BockShop.BLL.Specifications;
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
                StatusCode = 200,
                Message = "Create Order Successfully",
            });
        }
        [HttpGet]
        public async Task<ActionResult<GenResponse<IEnumerable<OrdersResponse>>>> GetAllOrder([FromQuery] OrderQueryParameters dto)
        {
            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            var response = await _orderService.GetAllOrdersAsync(dto, userId);
            return Ok(new GenResponse<IEnumerable<OrdersResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Orders Retrieved  Successfully",
                Data = response
            });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenResponse<OrderDetailsResponse>>> GetOrderById(int id)
        {
            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
            var response = await _orderService.GetOrderById(id, userId);

            return Ok(new GenResponse<OrderDetailsResponse>
            {
                Success = true,
                StatusCode = 200,
                Message = "Order Retrieved  Successfully",
                Data = response
            });
        }
        [HttpPut]
        public async Task<ActionResult<GenResponse<object>>> UpdateStatus([FromQuery] UpdateOrderStatusRequest dto)
        {
            await _orderService.UpdateOrderStatus(dto);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Update Status  Successfully",

            });
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult<GenResponse<object>>> CancelOrder(int Id)
        {
            string userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)!;
           await _orderService.CancelOrder(Id, userId);
            return Ok(new GenResponse<object>
            {
                Success = true,
                StatusCode = 200,
                Message = "Cancel Order  Successfully",

            });

        }
    }
}
