using BockShop.BLL.DTOs.Response;
using FluentValidation;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class RequestForgetPasswordDtoValidator : AbstractValidator<RequestForgetPasswordDto>
    {
        public RequestForgetPasswordDtoValidator()
        {
            RuleFor(e => e.Email)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Email Is Required")
               .EmailAddress()
               .WithMessage("Email Format is InValid");

        }
    }
}
