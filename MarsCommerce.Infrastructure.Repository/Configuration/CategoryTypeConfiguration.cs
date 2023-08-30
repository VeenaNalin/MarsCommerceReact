using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarsCommerce.Infrastructure.Repository.Configuration
{
    public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                 .HasColumnType("varchar(100)")
                 .HasMaxLength(100)
                 .IsUnicode()
                 .IsRequired();
        }
    }
}
