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
    internal class AuthorConfigrations : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(A => A.Name).IsRequired().HasMaxLength(50);
            builder.Property(A => A.Biography).HasMaxLength(500);

            builder.HasMany(A => A.Books).WithMany(B => B.Authors);
        }
    }
}
