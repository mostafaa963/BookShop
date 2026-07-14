using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.Context.Identity.Configuration
{
    public  class EntityCartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasOne<ApplicationUser>()
                  .WithOne(e => e.Cart)
                  .HasForeignKey<Cart>(e => e.UserId);

         

        }
    }
}
