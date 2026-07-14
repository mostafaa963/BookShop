using BockShop.BLL.DTOs.Request;
using BockShop.DAL.Specifications;
using BookShop.Domain.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class CouponSpecification : Specification<Coupon>
    {
        public CouponSpecification(CouponQueryParameters dto)
        {

            if (!string.IsNullOrEmpty(dto.Search))
                Criteria = e => e.Code.ToLower().Contains(dto.Search.ToLower());

            switch (dto.SortedBy?.ToLower())
            {
                case "code":
                    if (dto.Descending)
                        AddOrderByDescending(e => e.Code);
                    else
                        AddOrderBy(e => e.Code);
                    break;

                case "expiredate":
                    if (dto.Descending)
                        AddOrderByDescending(e => e.ExpiredAt);
                    else
                        AddOrderBy(e => e.ExpiredAt);
                    break;

                case "discount":
                    if (dto.Descending)
                        AddOrderByDescending(e => e.DiscountValue);
                    else
                        AddOrderBy(e => e.DiscountValue);
                    break;

                default:
                    AddOrderBy(e => e.Id);
                    break;
            }
            ApplyPaging((dto.PageNumber - 1) * dto.PageSize, dto.PageSize);
        }
    }
}
