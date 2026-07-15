using BockShop.BLL.DTOs.Request;
using BockShop.DAL.Specifications;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class OrderSpecification : Specification<Order>
    {
        public OrderSpecification(OrderQueryParameters dto, string userId)
        {
            
            Criteria = e => e.UserId == userId;

            switch (dto.OrderBy?.ToLower())
            {
                case "datetime":
                    {
                        if (dto.IsDescending)
                            AddOrderByDescending(e => e.CreatedAt);
                        else
                            AddOrderBy(e => e.CreatedAt);
                        break;
                    }
                case "price":
                    {
                        if (dto.IsDescending)
                            AddOrderByDescending(e => e.TotalPrice);
                        else
                            AddOrderBy(e => e.TotalPrice);

                        break;
                    }
                default:
                    {
                        AddOrderBy(e => e.Id);
                        break;
                    }

            }

            ApplyPaging((dto.PageNumber - 1) * dto.PageSize, dto.PageSize);
        }
    }
}
