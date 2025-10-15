using System.Diagnostics;

using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Services;

using InnoClinic.Authorization.Business.Configuration;
using InnoClinic.Authorization.Business.Helpers;
using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Business.Services;
using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Authorization.Infrastructure;
using InnoClinic.Authorization.Infrastructure.DataSeeders;
using InnoClinic.Shared;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Serilog;

namespace InnoClinic.Authorization.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppUrls.Initialize(builder.Configuration);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables();

            builder.Host.UseSerilog((context, configuration) =>
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.WithProperty("TraceId", () => Activity.Current?.Id)
            );

            var connectionString = builder.Configuration.GetConnectionString("AuthorizationDb");

            builder.Services.AddDbContext<AuthorizationContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly("InnoClinic.Authorization.Infrastructure"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddScoped<AccountsDataSeeder>();
            builder.Services.AddScoped<IProfilesApiHelper, ProfilesApiHelper>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IMessageService, EmailService>();
            builder.Services.AddTransient<IProfileService, ProfileService>();

            builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
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

            var licenseKey = builder.Configuration["IdentityServerCreds:LicenseKey"];
            builder.Services.AddIdentityServer(options =>
            {
                options.LicenseKey = licenseKey;
                options.IssuerUri = AppUrls.AuthUrl;
            })
                .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = b =>
                            b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly("InnoClinic.Authorization.Infrastructure"));

                        // Periodic removal of expired tokens/codes
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 3600; // seconds
                    })
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                .AddInMemoryClients(Configuration.GetClients())
                .AddInMemoryApiResources(Configuration.GetApiResources())
                .AddInMemoryApiScopes(Configuration.GetApiScopes())
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<Account>();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Auth.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });

            builder.Services.AddAuthorizationBuilder();

            builder.Services.AddAuthentication()
                .AddCookie("Cookies", options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowUI",
                    cors => 
                    cors.WithOrigins(AppUrls.ClientUiUrl, AppUrls.EmployeeUiUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            builder.Services.AddDbContext<DataProtectionKeysContext>(options =>
                options.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly("InnoClinic.Authorization.Infrastructure")));

            builder.Services.AddDataProtection()
                .PersistKeysToDbContext<DataProtectionKeysContext>()
                .SetApplicationName("InnoClinicAuth");

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var AuthdbContext = scope.ServiceProvider.GetRequiredService<AuthorizationContext>();
                var grantDb = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                var dataProtectionDb = scope.ServiceProvider.GetRequiredService<DataProtectionKeysContext>();
                try
                {
                    await AuthdbContext.Database.MigrateAsync();
                    await grantDb.Database.MigrateAsync();
                    await dataProtectionDb.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Could not migrate database", ex);
                }

                if (app.Environment.IsDevelopment())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<AccountsDataSeeder>();
                    await seeder.SeedAsync();
                }
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            var fh = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            // If you’re on docker bridge, clear KnownNetworks/Proxies so headers aren’t ignored
            fh.KnownNetworks.Clear();
            fh.KnownProxies.Clear();

            app.UseForwardedHeaders(fh);
            app.UseCors("AllowUI");
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

            app.UseStaticFiles();

            await app.RunAsync();
        }
    }
}