using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public  class ResetPasswordRequestDtoValidator:AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(e => e.Token)
                .NotEmpty()
                .WithMessage("Token  is Required ");

            RuleFor(e => e.Email)
                .NotEmpty()
                .WithMessage("Email is Required");

            RuleFor(e => e.NewPassword)
                .NotEmpty()
                .WithMessage("Password is Required");

            RuleFor(e => e.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm Password is Required")
                .Equal(e => e.NewPassword)
                .WithMessage("Confirm Password  Is Not Match");

                
        }
    }
}
