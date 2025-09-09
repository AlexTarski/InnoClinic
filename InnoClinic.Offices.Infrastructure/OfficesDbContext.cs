using InnoClinic.Offices.Domain;

using Microsoft.EntityFrameworkCore;

using MongoDB.EntityFrameworkCore.Extensions;

namespace InnoClinic.Offices.Infrastructure
{
    public class OfficesDbContext : DbContext
    {
        public DbSet<Office> Offices { get; set; }

        public OfficesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Office>(
                builder =>
                {
                    builder.ToCollection("offices");
                });
        }
    }
}