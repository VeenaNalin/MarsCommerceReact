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
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName)
               .HasMaxLength(250)
               .IsUnicode()
               .IsRequired();
            builder.Property(u => u.LastName)
               .HasMaxLength(250)
               .IsUnicode()
               .IsRequired();
            builder.HasOne(u => u.Cart)
              .WithOne(u => u.User);
        }
    }
}
