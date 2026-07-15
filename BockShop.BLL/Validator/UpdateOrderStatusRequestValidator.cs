using BockShop.BLL.Specifications;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusRequestValidator()
        {
            RuleFor(e => e.OrderId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("OrderId Is Required")
                .GreaterThan(0)
                .WithMessage("OrderId Must be Greater than 0");
            RuleFor(e => e.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("status is Required");
        }
    }
}
