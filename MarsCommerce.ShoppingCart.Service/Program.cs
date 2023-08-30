
using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using MarsCommerce.Infrastructure.Repository.Data;
using MarsCommerce.ShoppingCart.Service.Interfaces;
using MarsCommerce.ShoppingCart.Service.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

namespace MarsCommerce.ShoppingCart.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:44462", "https://localhost:7234").AllowAnyHeader().AllowAnyMethod();
                                  });
            });


            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(x =>
           x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
               options
               .UseLazyLoadingProxies()
               .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               );
            builder.Services.AddScoped<IRepository<ShoppingCartItem>, BaseRepository<ShoppingCartItem>>();
            builder.Services.AddScoped<IRepository<Product>, BaseRepository<Product>>();

            builder.Services.AddScoped<IRepository<Core.Models.ShoppingCart>, BaseRepository<Core.Models.ShoppingCart>>();

            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}