using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.Context.Identity.Configuration
{
    public  class EntityCartitemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {

            builder.HasOne(e => e.Book)
                .WithMany(e => e.cartItems)
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
