using InnoClinic.Offices.Domain;
using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<Office>();
        }
    }
}