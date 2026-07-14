using BockShop.DAL.Specifications;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.BLL.Specifications
{
    public class CartItemSpecification : Specification<CartItem>
    {
        public CartItemSpecification(int cartId)
        {
            AsNoTracking = true;
            Criteria = e=>e.CartId == cartId;
           AddInclude(e => e.Book);
           AddInclude(e => e.Book.Author);
        }

    }
}
