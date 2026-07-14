using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Interfaces
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponResponse>> GetAllCouponAsync(CouponQueryParameters dto);
        Task<CouponResponse> GetByIdAsync(int id);
        Task<AddCouponResponse> AddCouponAsync(AddCouponRequest dto);
        Task<UpdateCouponResponse> UpdateCoupon(int id, UpdateCouponRequest dto);
        Task DeleteCoupon(int id);
    }
}
