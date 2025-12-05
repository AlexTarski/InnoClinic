using InnoClinic.Appointments.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Appointments.Infrastructure
{
    public class AppointmentsContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentsContext()
        {
        }

        public AppointmentsContext(DbContextOptions<AppointmentsContext> options)
            : base(options) { }
    }
}