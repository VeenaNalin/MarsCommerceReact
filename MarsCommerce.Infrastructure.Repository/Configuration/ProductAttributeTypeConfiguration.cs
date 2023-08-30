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
    public class ProductAttributeTypeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttribute");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                 .HasColumnType("varchar(100)")
                 .HasMaxLength(100)
                 .IsUnicode()
                 .IsRequired();
            builder.HasMany(p => p.ProductAttributeMappings)
                .WithOne(p => p.ProductAttribute)
                .HasForeignKey(p => p.ProductAttributeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
