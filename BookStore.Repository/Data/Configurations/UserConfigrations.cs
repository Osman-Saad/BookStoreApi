using BookStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BookStore.Repository.Data.Configurations
{
    class UserConfigrations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.OwnsOne(u => u.Address, a =>
            {
                a.Property(x => x.City).IsRequired().HasMaxLength(100);
                a.Property(x => x.Street).IsRequired().HasMaxLength(200);
                a.Property(x => x.BuildingNumber).IsRequired().HasMaxLength(50);
            });
            builder.Property(U => U.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(U => U.LastName).IsRequired().HasMaxLength(50);

            builder.HasMany(U => U.Orders).WithOne(O => O.User).HasForeignKey(O => O.UserId).OnDelete(DeleteBehavior.SetNull);
            builder.OwnsMany(U => U.RefreshTokens);
        }
    }
}
