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
    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.ToTable("Product");
           builder.HasKey(p => p.Id);
           builder.Property(p => p.Name)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();
           builder.Property(p => p.Description)
                .HasMaxLength(500)
                .IsUnicode()
                .IsRequired();
            builder.Property(p => p.StockCount)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(p => p.ImageUrl)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(p => p.StockKeepingUnit)
                .HasMaxLength(15)
                .IsRequired();
            builder.Property(p => p.Rating)
                .HasColumnType("decimal(1,1)")
                .IsRequired(false);
            builder.HasMany(p => p.Attributes)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        

        }
    }
}
