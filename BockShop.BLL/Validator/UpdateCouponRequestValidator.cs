using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class UpdateCouponRequestValidator : AbstractValidator<UpdateCouponRequest>
    {
        public UpdateCouponRequestValidator()
        {
            RuleFor(e => e.Code)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Code is Required")
               .MaximumLength(20)
               .WithMessage("Maximum length is 20");
            RuleFor(e => e.UsageLimit)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Usage Limiting is Required")
                .LessThan(100)
                .WithMessage("Usage Limiting Should be LessThan 100 ");
            RuleFor(e => e.ExpiredAt)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("Expired Data")
                .WithMessage("Expired Data is Required");
            RuleFor(e => e.DiscountValue)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("DiscountValue is Required")
                .LessThanOrEqualTo(70)
                .WithMessage("DiscountValue Should be LessThan 70 ");
            RuleFor(e => e.IsActive)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("DiscountValue is Required");


        }
    }
}
