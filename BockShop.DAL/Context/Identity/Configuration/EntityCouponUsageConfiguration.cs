using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.Context.Identity.Configuration
{
    public class EntityCouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
    {
        public void Configure(EntityTypeBuilder<CouponUsage> builder)
        {
            builder.HasOne<ApplicationUser>()
                .WithMany(e => e.CouponUsages)
                .HasForeignKey(e => e.userId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e=>e.Coupon)
                .WithMany(e=>e.CouponUsages)
                .HasForeignKey(e=>e.CouponId)
                .OnDelete(DeleteBehavior.Cascade);
                


        }
    }
}
