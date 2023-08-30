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
    public class ProductAttributeMappingTypeConfiguration : IEntityTypeConfiguration<ProductAttributeMapping>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeMapping> builder)
        {
            builder.ToTable("ProductAttributeMapping");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ProductId)
                .IsRequired();
            builder.Property(p=> p.ProductAttributeId)
                .IsRequired();
            builder.Property(p => p.ProductAttributeValue)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();
            builder.HasOne(p => p.Product)
                .WithMany(p => p.Attributes)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.ProductAttribute)
                .WithMany(p => p.ProductAttributeMappings)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
