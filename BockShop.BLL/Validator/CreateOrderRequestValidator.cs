using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(e => e.CouponCode)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Coupon Code is Required");
        }
    }
}
