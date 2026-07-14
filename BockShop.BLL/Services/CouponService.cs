using BockShop.BLL.DTOs.Request;
using BockShop.BLL.DTOs.Response;
using BockShop.BLL.Exceptions;
using BockShop.BLL.Interfaces;
using BockShop.BLL.Specifications;
using BockShop.DAL.Interfaces;
using BookShop.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;


namespace BockShop.BLL.Services
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCouponRequest> _addCouponRequestValidator;
        private readonly IValidator<UpdateCouponRequest> _updateCouponRequestValidator;

        public CouponService(IValidator<UpdateCouponRequest> updateCouponRequestValidator, IValidator<AddCouponRequest> addCouponRequestValidator, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _addCouponRequestValidator = addCouponRequestValidator;
            _updateCouponRequestValidator = updateCouponRequestValidator;
        }
        public async Task<IEnumerable<CouponResponse>> GetAllCouponAsync(CouponQueryParameters dto)
        {
            var specificationCoupon = new CouponSpecification(dto);
            var coupons = await _unitOfWork.Coupon.GetAll(specificationCoupon);
            var response = coupons.Select(e => new CouponResponse
            {
                code = e.Code,
                ExpiredAt = e.ExpiredAt,
                UsageLimit = e.UsageLimit,
                CountUsed = e.CountUsed,
                IsActive = e.IsActive,
            });
            return response;
        }
        public async Task<CouponResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException(new[]
                {
                    new ValidationFailure (nameof(id),"Id Must be Greater Than 0"),
                });


            var coupon = await _unitOfWork.Coupon.GetByIdAsync(id);
            if (coupon == null)
                throw new NotFoundException("Coupon is Not Found");
            return new CouponResponse
            {
                code = coupon.Code,
                ExpiredAt = coupon.ExpiredAt,
                UsageLimit = coupon.UsageLimit,
                CountUsed = coupon.CountUsed,
                IsActive = coupon.IsActive,
            };

        }
        public async Task<AddCouponResponse> AddCouponAsync(AddCouponRequest dto)
        {
            var validator = await _addCouponRequestValidator.ValidateAsync(dto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);

            var exists = await _unitOfWork.Coupon.GetFirstOneAsync(e => e.Code.ToLower() == dto.Code.ToLower().Trim());
            if (exists != null)
                throw new ValidationException(new[] {
                    new ValidationFailure(nameof(dto.Code),"Code is Already Exist")
                });
            var newCoupon = new Coupon
            {
                Code = dto.Code,
                UsageLimit = dto.UsageLimit,
                DiscountValue = dto.DiscountValue,
                ExpiredAt = dto.ExpiredAt,
                IsActive = true,
                CountUsed = 0,
            };
            await _unitOfWork.Coupon.AddAsync(newCoupon);
            await _unitOfWork.SaveChangeAsync();
            return new AddCouponResponse
            {
                Id = newCoupon.Id,
                Code = newCoupon.Code,
                ExpiredAt = newCoupon.ExpiredAt,
            };
        }
        public async Task<UpdateCouponResponse> UpdateCoupon(int id, UpdateCouponRequest dto)
        {
            var validator = await _updateCouponRequestValidator.ValidateAsync(dto);
            if (!validator.IsValid)
                throw new ValidationException(validator.Errors);
            var coupon = await _unitOfWork.Coupon.GetByIdAsync(id);
            if (coupon == null)
                throw new NotFoundException("Coupon is Not Found");
            var exists = await _unitOfWork.Coupon.GetFirstOneAsync(e =>
            e.Code.ToLower().Trim() == dto.Code.ToLower().Trim() && e.Id != id);
            if (exists != null)
                throw new ValidationException(new[] {
                    new ValidationFailure(nameof(dto.Code),"Code is Already Exist")
                });
            coupon.Code = dto.Code;
            coupon.ExpiredAt = dto.ExpiredAt;
            coupon.UsageLimit = dto.UsageLimit;
            coupon.DiscountValue = dto.DiscountValue;
            coupon.IsActive = dto.IsActive;
            _unitOfWork.Coupon.Update(coupon);
            await _unitOfWork.SaveChangeAsync();

            return new UpdateCouponResponse
            {
                Code = coupon.Code,
                DiscountValue = coupon.DiscountValue,
                ExpiredAt = coupon.ExpiredAt,
                UsageLimit = coupon.UsageLimit,
                IsActive = coupon.IsActive,
            };

        }
        public async Task DeleteCoupon(int id)
        {
            if (id <= 0)
                throw new ValidationException(new[] {
                new ValidationFailure(nameof(id),"Id Must be greater than 0")
                });

            var coupon = await _unitOfWork.Coupon.GetByIdAsync(id);
            if (coupon == null)
                throw new NotFoundException("Coupon does not exist.");
            _unitOfWork.Coupon.Delete(coupon);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
