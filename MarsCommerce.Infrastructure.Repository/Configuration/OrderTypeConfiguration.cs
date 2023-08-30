using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MarsCommerce.Infrastructure.Repository.Configuration
{
    public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.UserId)
            .IsRequired();
            builder.Property(a => a.OrderDate)
            .IsRequired();
            builder.Property(a => a.PaymentInfoId)
            .IsRequired();
            builder.Property(a => a.DeliveryCharge)
            .IsRequired();
            builder.Property(a => a.AddressId)
            .IsRequired();
            builder.Property(a => a.OrderTotal)
            .IsRequired();
            builder.HasMany(a => a.Items)
            .WithOne(a => a.Order)
            .HasForeignKey(a => a.OrderId)
           .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.User)
           .WithMany(a => a.Orders)
           .HasForeignKey(a => a.UserId)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}