using BookShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BockShop.DAL.Context.Identity.Configuration
{
    public class EntityFavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasOne<ApplicationUser>()
                .WithMany(e => e.favorites)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Book)
             .WithMany(e => e.favorites)
             .HasForeignKey(e => e.BookId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
