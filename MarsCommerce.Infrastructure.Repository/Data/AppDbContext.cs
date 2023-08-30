using MarsCommerce.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace MarsCommerce.Infrastructure.Repository.Data
{
    public class AppDbContext : DbContext
    {

      public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
      {
        
      }

      public DbSet<Product> Products => Set<Product>();
      public DbSet<ShoppingCart> Carts => Set<ShoppingCart>();
      public DbSet<User> Users => Set<User>();
      public DbSet<ShoppingCartItem> ShoppingCartItems=> Set<ShoppingCartItem>();
      public DbSet<Category> Categories=> Set<Category>();




        protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        base.OnModelCreating(modelBuilder);

       // modelBuilder.Entity<ShoppingCartEntity>().Navigation(e => e.Items).AutoInclude();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      }

  
    }
}