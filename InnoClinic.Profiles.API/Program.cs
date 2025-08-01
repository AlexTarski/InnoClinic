using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Business.Services;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;
using InnoClinic.Profiles.Infrastructure;
using InnoClinic.Profiles.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;
using InnoClinic.Profiles.Domain.Entities.Users;

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

            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<ICrudRepository<Doctor>, DoctorsRepository>();
            builder.Services.AddScoped<ICrudRepository<Patient>, PatientsRepository>();
            builder.Services.AddScoped<ICrudRepository<Receptionist>, ReceptionistsRepository>();
            builder.Services.AddScoped<ICrudRepository<Account>, AccountsRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
                    
                    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                    await seeder.SeedAsync();
                }
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("AllowAll");

            await app.RunAsync();
        }
    }
}
