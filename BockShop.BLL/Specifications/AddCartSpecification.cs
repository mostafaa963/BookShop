using BockShop.DAL.Specifications;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class AddCartSpecification : Specification<Cart>
    {
        public AddCartSpecification(string userId)
        {
            Criteria = e => e.UserId == userId;
            AddInclude(e => e.cartItems);
        }
    }
}
