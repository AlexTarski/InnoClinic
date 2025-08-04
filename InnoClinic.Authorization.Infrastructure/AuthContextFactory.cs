using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace InnoClinic.Authorization.Infrastructure
{
    public class AuthorizationContextFactory : IDesignTimeDbContextFactory<AuthorizationContext>
    {
        public AuthorizationContext CreateDbContext(string[] args)
        {
            // Get the base path for locating appsettings.json
            var basePath = Directory.GetCurrentDirectory();

            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Read connection string
            var connectionString = config.GetConnectionString("AuthorizationContextDb");

            // Configure DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AuthorizationContext>();
            optionsBuilder.UseSqlServer(connectionString,
                x => x.MigrationsAssembly("InnoClinic.Authorization.Infrastructure"));

            return new AuthorizationContext(optionsBuilder.Options);
        }
    }
}