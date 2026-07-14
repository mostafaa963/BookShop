using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class AddCartItemRequestValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemRequestValidator()
        {
            RuleFor(e => e.BookId)
                .Must(e => e > 0)
                .WithMessage("Book Id Must be greater Than  0");
            RuleFor(e => e.Quantity)
                .Must(e => e > 0)
                .WithMessage(" Quantity Must be greater Than  0")
                .LessThan(20)
                .WithMessage("Quantity Must be LessThan 20");
        }
    }
}
