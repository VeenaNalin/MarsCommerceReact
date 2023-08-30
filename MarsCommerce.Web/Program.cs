using Ardalis.ApiClient;
using FluentValidation;
using MarsCommerce.Web.View_Models;
using MarsCommerce.Web.View_Models.ValidatorsVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using MarsCommerce.Core.Interfaces;

namespace MarsCommerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var allowSpecificOrigins = "allowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
            var baseUrl = builder.Configuration.GetSection("ApplicationBaseUrl:BaseUrl").Value;
            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options);

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidIssuer = "https://marscad.b2clogin.com/";
            },
             options => { builder.Configuration.Bind("AzureAdB2C", options); });
            // End of the Microsoft Identity platform block    

            builder.Services.AddControllers();


            builder.Services.AddControllersWithViews();

            //Ardalis Api Client Registration
            builder.Services.AddScoped(sp => HttpClientBuilder());
            builder.Services.AddScoped<HttpService>();
            builder.Services.AddSingleton<IValidator<ProductVM>, ProductValidatorVM>();
            builder.Services.AddSingleton<IValidator<UserVM>, UserValidatorVM>();
            builder.Services.AddCors(options =>
            {

                options.AddPolicy(name: allowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins(baseUrl)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();


            app.UseAuthorization();
            app.UseCors(allowSpecificOrigins);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }

        private static HttpClient HttpClientBuilder()
        {
            var client = new HttpClient();
            return client;
        }
    }
}