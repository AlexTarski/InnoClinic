using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using IdentityServer4.Models;
using InnoClinic.Authorization.Infrastructure;
using InnoClinic.Authorization.Infrastructure.Repositories;
using InnoClinic.Authorization.Domain;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Services;
using Microsoft.AspNetCore.Identity;

namespace InnoClinic.Authorization.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AuthorizationContextDb");

            builder.Services.AddDbContext<AuthorizationContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly("InnoClinic.Authorization.Infrastructure"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<ICrudRepository<YourEntity>, YourEntityRepository>();
            builder.Services.AddScoped<IYourEntityService, YourService>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddIdentity<YourEntity, IdentityRole<Guid>>(config =>
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AuthorizationContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer()
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddInMemoryApiResources(Configuration.GetApiResources())
                .AddInMemoryApiScopes(Configuration.GetApiScopes())
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<YourEntity>();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Profiles.API.Identity.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    cors => cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationContext>();

                if (!await dbContext.Database.CanConnectAsync())
                {
                    await dbContext.Database.MigrateAsync();

                    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                    await seeder.SeedAsync();
                }
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseIdentityServer();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseCors("AllowAll");

            await app.RunAsync();
        }
    }
}