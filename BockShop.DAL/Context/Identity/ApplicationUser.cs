using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BockShop.DAL.Context.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public Cart Cart { get; set; }
        public string? Address { get; set; }
        public IEnumerable<Author>? Authors { get; set; }
        public IEnumerable<Favorite>? favorites { get; set; }
        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
        public IEnumerable<CouponUsage>? CouponUsages { get; set; }
        public IEnumerable<Order>? Orders { get; set; }
    }
}
