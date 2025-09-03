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
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.OrderStatus)
                .HasConversion(OrderStatus => OrderStatus.ToString(),
                               OrderStatusString => (OrderStatus)Enum.Parse(typeof(OrderStatus), OrderStatusString));
            builder.Property(O => O.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasMany(O => O.Items).WithOne(OI=>OI.Order).HasForeignKey(O => O.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
