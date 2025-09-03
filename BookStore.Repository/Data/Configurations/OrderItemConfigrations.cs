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
    internal class OrderItemConfigrations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(OI => OI.Id);
            builder.Property(OI => OI.Quantity).IsRequired();
            builder.Property(OI => OI.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(OI=>OI.BookName).IsRequired().HasMaxLength(50);
            builder.HasOne(OI=>OI.Order).WithMany(O => O.Items).HasForeignKey(OI => OI.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
