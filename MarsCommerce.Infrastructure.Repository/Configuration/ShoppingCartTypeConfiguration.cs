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
    

        public class ShoppingCartTypeConfiguration : IEntityTypeConfiguration<ShoppingCart>
        {
            public void Configure(EntityTypeBuilder<ShoppingCart> builder)
            {
                builder.ToTable("ShoppingCart");
                builder.HasKey(c => c.Id);
               
                builder.Property(c => c.UserId)
                  .IsRequired();

                builder.HasMany(u => u.Items)
              .WithOne(u => u.ShoppingCart);



             }
        }
    
}
