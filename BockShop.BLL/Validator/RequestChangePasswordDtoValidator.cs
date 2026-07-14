using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class RequestChangePasswordDtoValidator : AbstractValidator<RequestChangePasswordDto>
    {
        public RequestChangePasswordDtoValidator()
        {
            RuleFor(e => e.EmailOrUserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Email Or UserName Is Required");

            RuleFor(e => e.Password)
                .NotEmpty()
                .WithMessage("Password is Required");
            RuleFor(e => e.NewPassword)
                .NotEmpty()
                .WithMessage("NewPassword is Required");
            RuleFor(e => e.ConfirmPassword)
                .NotEmpty()
                .WithMessage("ConfirmPassword is Required")
                .Equal(e => e.NewPassword)
                .WithMessage("Password Not Match");

        }

    }
}
