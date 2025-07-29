using System.Reflection;
using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.App.Services;
using InnoClinic.Profiles.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using InnoClinic.Profiles.Infrastructure;

namespace InnoClinic.Profiles.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("config.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var connectionString = builder.Configuration.GetConnectionString("ProfilesContextDb");
            
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

            builder.Services.AddScoped<IProfileRepository, ProfilesRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientService,  PatientService>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(cfg =>
                    cfg.SerializerSettings
                    .ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

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
                    await dbContext.Database.MigrateAsync();
                }
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
