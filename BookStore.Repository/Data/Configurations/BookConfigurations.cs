using BookStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Data.Configurations
{
    internal class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(B => B.Name).IsRequired().HasMaxLength(50);
            builder.Property(B=>B.Description).HasMaxLength(500);
            builder.Property(B => B.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(B => B.Publisher).WithMany(P=>P.Books).HasForeignKey(B => B.PublisherId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(B => B.Category).WithMany(C=>C.Books).HasForeignKey(B => B.CategoryId).OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(B => B.Authors).WithMany(A => A.Books);
        }
    }
}
