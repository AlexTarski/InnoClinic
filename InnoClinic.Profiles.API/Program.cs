using System.Diagnostics;
using System.Security.Claims;

using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Services;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Infrastructure;
using InnoClinic.Profiles.Infrastructure.Repositories;
using InnoClinic.Shared;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using Serilog;

namespace InnoClinic.Profiles.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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

            var connectionString = builder.Configuration.GetConnectionString("ProfilesDb");

            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddDbContext<ProfilesContext>(options =>
            {
                options.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly("InnoClinic.Profiles.Infrastructure"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<IDoctorsRepository, DoctorsRepository>();
            builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
            builder.Services.AddScoped<IReceptionistsRepository, ReceptionistsRepository>();
            builder.Services.AddScoped<IProfilesRepository, ProfilesRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
            builder.Services.AddScoped<IProfilesService, ProfilesService>();

            builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(cfg =>
                    cfg.SerializerSettings
                    .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    cors => cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var authUrl = builder.Configuration.GetConnectionString("AuthUrl");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authUrl;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidAudience = "profiles",
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = "name"
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProfilesContext>();

                if (!await dbContext.Database.CanConnectAsync())
                {
                    try
                    {
                        await dbContext.Database.MigrateAsync();
                    }
                    catch(Exception ex)
                    {
                        throw new InvalidOperationException("Could not migrate database", ex);
                    }

                    if (app.Environment.IsDevelopment())
                    {
                        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                        await seeder.SeedAsync();
                    }
                }
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}