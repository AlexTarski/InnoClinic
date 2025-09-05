using System.Diagnostics;

using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Business.Services;
using InnoClinic.Offices.Domain;
using InnoClinic.Offices.Infrastructure;

using Microsoft.EntityFrameworkCore;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using Serilog;

namespace InnoClinic.Offices.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

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


            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            var mongoDBSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddDbContext<OfficesDbContext>(options =>
                options.UseMongoDB(mongoDBSettings.MongoDbUri ?? "", mongoDBSettings.DatabaseName ?? ""));

            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<IOfficesRepository, OfficesRepository>();
            builder.Services.AddScoped<IOfficeService, OfficeService>();

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

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                await seeder.SeedAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
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