using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace InnoClinic.Authorization.Infrastructure
{
    public class AuthorizationContextFactory : IDesignTimeDbContextFactory<AuthorizationContext>
    {
        public AuthorizationContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            
            var connectionString = config.GetConnectionString("AuthorizationDb");
            
            var optionsBuilder = new DbContextOptionsBuilder<AuthorizationContext>();
            optionsBuilder.UseSqlServer(connectionString,
                x => x.MigrationsAssembly("InnoClinic.Authorization.Infrastructure"));

            return new AuthorizationContext(optionsBuilder.Options);
        }
    }
}