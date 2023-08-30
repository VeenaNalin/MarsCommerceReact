using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Infrastructure.Repository.Configuration
{
    public class PaymentInfoTypeConfiguration : IEntityTypeConfiguration<PaymentInfo>
    {
        public void Configure(EntityTypeBuilder<PaymentInfo> builder)
        {
            builder.ToTable("PaymentInfo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PaymentMethod)
                 .HasColumnType("varchar(100)")
                 .HasMaxLength(100)
                 .IsUnicode()
                 .IsRequired();
        }
    }
}