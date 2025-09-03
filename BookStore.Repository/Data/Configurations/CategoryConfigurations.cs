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
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(C=>C.Name).IsRequired().HasMaxLength(100);
            builder.Property(C => C.Description).HasMaxLength(500);
            builder.HasMany(C => C.Books).WithOne(B => B.Category).HasForeignKey(B => B.CategoryId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
