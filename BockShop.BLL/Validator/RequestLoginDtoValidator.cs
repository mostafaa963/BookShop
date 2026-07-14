using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class RequestLoginDtoValidator : AbstractValidator<RequestLoginDto>
    {
        public RequestLoginDtoValidator()
        {
            RuleFor(e => e.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("UserName is Required")
                .MinimumLength(5)
                .WithMessage("Minimum size is 5");

            RuleFor(e => e.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Password is Required");

        }
    }
}
