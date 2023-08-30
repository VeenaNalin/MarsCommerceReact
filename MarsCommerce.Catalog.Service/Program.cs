using FluentValidation;
using MarsCommerce.Catalog.Service.Interfaces;
using MarsCommerce.Core.Interfaces;
using MarsCommerce.Core.Models;
using MarsCommerce.Core.Validators;
using MarsCommerce.Infrastructure.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace MarsCommerce.Catalog.Service
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
                                      policy.WithOrigins("https://localhost:44462").AllowAnyHeader().AllowAnyMethod();
                                  });
            });
            //Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseLazyLoadingProxies().
            UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
            builder.Services.AddScoped<IRepository<Product>, BaseRepository<Product>>();
            builder.Services.AddScoped<IRepository<Address>, BaseRepository<Address>>();
            builder.Services.AddScoped<IRepository<ProductAttribute>, BaseRepository<ProductAttribute>>();
            builder.Services.AddScoped<IRepository<Category>, BaseRepository<Category>>();
            builder.Services.AddScoped<ICatalogService, CatalogService>();
            builder.Services.AddSingleton<IValidator<Product>, ProductValidator>();

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