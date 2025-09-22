using System;
using System.Diagnostics;
using System.Threading.Tasks;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Business.Services;
using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Infrastructure;
using InnoClinic.Documents.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace InnoClinic.Documents.API
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

            var connectionString = builder.Configuration.GetConnectionString("DocumentsDb");

            builder.Services.AddDbContext<DocumentsContext>(options =>
            {
                options.UseSqlServer(connectionString,
                            x => x.MigrationsAssembly("InnoClinic.Documents.Infrastructure"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });

                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DocumentsContext>();

                    if (!await dbContext.Database.CanConnectAsync())
                    {
                        try
                        {
                            await dbContext.Database.MigrateAsync();
                        }
                        catch
                        {
                            throw new InvalidOperationException("Could not migrate database");
                        }

                        if (app.Environment.IsDevelopment())
                        {
                            var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                            await seeder.SeedAsync();
                        }
                    }
                }
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}