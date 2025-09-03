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
    internal class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.Property(P => P.Name).IsRequired().HasMaxLength(50);
            builder.Property(P => P.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(P => P.Email).IsRequired().HasMaxLength(100);
            builder.HasMany(P => P.Books).WithOne(B => B.Publisher).HasForeignKey(B => B.PublisherId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
