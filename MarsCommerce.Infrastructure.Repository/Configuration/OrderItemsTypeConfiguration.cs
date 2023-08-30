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
    public class OrderItemsTypeConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.OrderId)
            .IsRequired();
            builder.Property(a => a.ProductId)

            .IsRequired();
            builder.Property(a => a.Quantity)
            .IsRequired();
            builder.Property(a => a.UnitPrice)

            .IsRequired();
            builder.Property(a => a.TotalPrice)
            .IsRequired();
            

        }
    }
}