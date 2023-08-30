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
    public class AddressTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Mobile)
            .IsRequired();
            builder.Property(a => a.Address1)
            .HasMaxLength(250)
            .IsUnicode()
            .IsRequired();
            builder.Property(a => a.Address2)
            .HasMaxLength(250)
            .IsUnicode()
            .IsRequired();
            builder.Property(a => a.City)
            .HasMaxLength(250)
            .IsUnicode()
            .IsRequired();
            builder.Property(a => a.PinCode)
            .IsRequired();
            builder.Property(a => a.State)
            .HasMaxLength(250)
            .IsUnicode()
            .IsRequired();
            builder.Property(a => a.IsDefaultAddress)
            .IsRequired();
           builder.HasOne(a => a.User)
            .WithMany(a => a.Addresses)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}