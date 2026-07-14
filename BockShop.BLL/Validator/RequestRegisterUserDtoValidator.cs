using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public  class RequestRegisterUserDtoValidator :AbstractValidator<RequestRegisterUserDto>
    {
        public RequestRegisterUserDtoValidator()
        {
            RuleFor(e => e.UserName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("UserName Is Required ")
               .MinimumLength(5)
               .WithMessage("Minimum Length Is 5 ");
            RuleFor(e => e.FullName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("FullName  Is Required ")
               .MinimumLength(10)
               .WithMessage("Minimum Length Is 10");
            RuleFor(e => e.Email)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Email Is Required ")
               .EmailAddress()
               .WithMessage("Email format is In Valid ");
            RuleFor(e => e.Password)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Password Is Required ");
            RuleFor(e => e.ConfirmPassword)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Confirm Is Required ")
               .Equal(e => e.Password)
               .WithMessage("Password Not Match ");

                

        }
    }
}
