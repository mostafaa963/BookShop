using BockShop.BLL.Interfaces;
using BockShop.BLL.Services;
using BockShop.DAL.Interfaces;
using BockShop.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShop.Api
{
    public static class DependenceInjection
    {
        public static void  Configure(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IBookService, BookService>();
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<ITokenService, JwtTokenService>();
            Services.AddScoped<IEmailService, EmailService>();
            Services.AddScoped<ICouponService, CouponService>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddScoped<IOrderService, OrderService>();
        }
    }
}
