using InnoClinic.Services.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.Infrastructure
{
    public class ServicesContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public ServicesContext()
        {
        }

        public ServicesContext(DbContextOptions<ServicesContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>()
                .HasOne(service => service.Category)
                .WithMany(category => category.Services)
                .HasForeignKey(service => service.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Service>()
                .HasOne(service => service.Specialization)
                .WithMany(specialization => specialization.Services)
                .HasForeignKey(service => service.SpecializationId)
                .IsRequired();
        }
    }
}