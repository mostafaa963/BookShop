using BockShop.BLL.DTOs.Request;
using BockShop.DAL.Specifications;
using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class OrderWithItemSpecification : Specification<Order>
    {
        public OrderWithItemSpecification(int orderId, string userId)
        {
            Criteria = e => e.UserId == userId && e.Id == orderId;
            Include = e => e.Include(e => e.orderItems!)
            .ThenInclude(e => e.Book);
        }
    }
}
