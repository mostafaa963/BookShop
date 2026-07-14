using Azure;
using BockShop.BLL.Common;
using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet]
        public async Task<ActionResult<GenResponse<IEnumerable<CouponResponse>>>> GetAll([FromQuery] CouponQueryParameters coupon)
        {

            var response = await _couponService.GetAllCouponAsync(coupon);

            return Ok(new GenResponse<IEnumerable<CouponResponse>>
            {
                StatusCode = 200,
                Success = true,
                Message = "Coupons  Retrieved Successfully",
                Data = response
            });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenResponse<CouponResponse>>> GetById(int id)
        {
            var response = await _couponService.GetByIdAsync(id);

            return Ok(new GenResponse<CouponResponse>
            {
                StatusCode = 200,
                Success = true,
                Message = "Coupon  Retrieved Successfully",
                Data = response

            });
        }
        [HttpPost]
        public async Task<ActionResult<GenResponse<AddCouponResponse>>> AddCoupon(AddCouponRequest dto)
        {
            var response = await _couponService.AddCouponAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, new GenResponse<AddCouponResponse>
            {

                StatusCode = 201,
                Success = true,
                Message = "Coupon created successfully.",
                Data = response
            });
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GenResponse<UpdateCouponResponse>>> UpdateCoupon(int id, UpdateCouponRequest dto)
        {
            var response = await _couponService.UpdateCoupon(id, dto);
            return Ok(new GenResponse<UpdateCouponResponse>
            {
                StatusCode = 200,
                Success = true,
                Message = "Update Coupon Successfully",
                Data = response

            });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<GenResponse<object>>> Delete(int id)
        {
            await _couponService.DeleteCoupon(id);
            return Ok(new GenResponse<object>
            {
                StatusCode = 200,
                Success = true,
                Message = "Delete Coupon Successfully",
            });
        }
    }
}
