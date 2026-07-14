using BockShop.DAL.Context;
using BockShop.DAL.Context.Identity;
using BockShop.DAL.Interfaces;
using BockShop.DAL.Repositories;
using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace BockShop.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public IRepository<Book> Book { get; private set; }

        public IRepository<Author> Author { get; private set; }

        public IRepository<Category> Category { get; private set; }
        public IRepository<RefreshToken> RefreshToken { get; private set; }
        public IRepository<Publisher> Publisher { get; private set; }

        public IRepository<Favorite> Favorite { get; private set; }

        public IRepository<Cart> Cart { get; private set; }

        public IRepository<CartItem> CartItem { get; private set; }

        public IRepository<Coupon> Coupon { get; private set; }

        public IRepository<CouponUsage> CouponUsage { get; private set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            Book = new Repository<Book>(_applicationDbContext);
            Category = new Repository<Category>(_applicationDbContext);
            Author = new Repository<Author>(_applicationDbContext);
            Publisher = new Repository<Publisher>(_applicationDbContext);
            Favorite = new Repository<Favorite>(_applicationDbContext);
            CartItem = new Repository<CartItem>(_applicationDbContext);
            Cart = new Repository<Cart>(_applicationDbContext);
            RefreshToken = new Repository<RefreshToken>(_applicationDbContext);
            Coupon = new Repository<Coupon>(_applicationDbContext);
            CouponUsage = new Repository<CouponUsage>(_applicationDbContext);

        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _applicationDbContext.SaveChangesAsync();
        }
    }
}
