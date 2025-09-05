using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Business.Services;
using InnoClinic.Offices.Domain;
using InnoClinic.Offices.Infrastructure;

using Microsoft.EntityFrameworkCore;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

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

            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            var mongoDBSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
            builder.Services.AddDbContext<OfficesDbContext>(options =>
                options.UseMongoDB(mongoDBSettings.MongoDbUri ?? "", mongoDBSettings.DatabaseName ?? ""));

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
                var dbContext = scope.ServiceProvider
                                     .GetRequiredService<OfficesDbContext>();

                // if there are no Office documents, the DB (and first collection) will be
                // created implicitly on the first write
                if (!await dbContext.Offices.AnyAsync())
                {
                    var sampleOffices = new[]
                    {
                        new Office
                        {
                            Id = Guid.NewGuid(),
                            Address   = new Address
                            {
                                City        = "City B",
                                Street      = "Street B",
                                HouseNumber    = "1B",
                                OfficeNumber  = "222"
                            },
                            RegistryPhoneNumber  = "+0-000-0000000",
                            isActive = false
                        },
                        new Office
                        {
                            Id        = Guid.NewGuid(),
                            RegistryPhoneNumber      = "+1-111-1111111",
                            Address   = new Address
                            {
                                City        = "City A",
                                Street      = "Street A",
                                HouseNumber    = "1A",
                                OfficeNumber  = "111"
                            },
                            isActive = true
                        }
                    };

                    await dbContext.Offices.AddRangeAsync(sampleOffices);
                    await dbContext.SaveChangesAsync();
                }
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