using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Infrastructure.Repository.Configuration
{
    public class ShoppingCartItemTypeConfiguration : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.ToTable("ShoppingCartItem");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CartId)
             .IsUnicode()
           .IsRequired();
            builder.Property(c => c.ProductID)
             .IsRequired();
            builder.Property(c => c.Quntity)
           .IsRequired();
            builder.Property(c => c.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(c => c.TotalPrice)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
            builder.HasOne(u => u.ShoppingCart)
            .WithMany(u => u.Items)
            .HasForeignKey(u => u.CartId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Product)
           .WithMany(u => u.Items)
           .HasForeignKey(u => u.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
