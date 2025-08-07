using Microsoft.EntityFrameworkCore;
using System.Reflection;
using InnoClinic.Authorization.Infrastructure;
using InnoClinic.Authorization.Domain.Entities.Users;
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

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.Services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddIdentity<Account, IdentityRole<Guid>>(config =>
                {
                    config.Password.RequiredLength = 6;
                    config.Password.RequireDigit = true;
                    config.Password.RequireNonAlphanumeric = true;
                    config.Password.RequireUppercase = true;
                    config.Password.RequireLowercase = true;
                    config.User.RequireUniqueEmail = true;
                    config.User.AllowedUserNameCharacters = null; // Allow any characters in username
                })
                .AddEntityFrameworkStores<AuthorizationContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer()
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddInMemoryApiResources(Configuration.GetApiResources())
                .AddInMemoryApiScopes(Configuration.GetApiScopes())
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<Account>();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Profiles.API.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });
            
            builder.Services.AddAuthentication()
                .AddCookie("Cookies", options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    cors => cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthorizationContext>();

                if (!await dbContext.Database.CanConnectAsync())
                {
                    await dbContext.Database.MigrateAsync();

                    //var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                    //await seeder.SeedAsync();
                }
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseCookiePolicy();
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Authorization API v1");
                    options.RoutePrefix = string.Empty; 
                });
            }

            await app.RunAsync();
        }
    }
}