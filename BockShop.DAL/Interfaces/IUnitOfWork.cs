using BockShop.DAL.Context.Identity;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Book> Book { get; }
        IRepository<Author> Author { get; }
        IRepository<Category> Category { get; }
        IRepository<Publisher> Publisher { get; }
        IRepository<Favorite> Favorite { get; }
        IRepository<Cart> Cart { get; }
        IRepository<CartItem> CartItem { get; }
        IRepository<RefreshToken> RefreshToken { get; }
        IRepository<Coupon> Coupon { get; }
        IRepository<CouponUsage> CouponUsage { get; }
        IRepository<Order> Order { get; }
        IRepository<OrderItem> OrderItem { get; }
        Task<int> SaveChangeAsync();

    }
}
