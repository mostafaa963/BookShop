using BockShop.BLL.DTOs.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Validator
{
    public class RequestFavoriteItemDtoValidator : AbstractValidator<RequestFavoriteItemDto>
    {
        public RequestFavoriteItemDtoValidator()
        {
            RuleFor(e => e.BookId)
                .NotEmpty()
                .WithMessage("Book is Required ");
        }
    }
}
