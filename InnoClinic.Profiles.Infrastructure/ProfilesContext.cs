using InnoClinic.Profiles.Domain.Entities;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class ProfilesContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public ProfilesContext()
    {
    }

    public ProfilesContext(DbContextOptions<ProfilesContext> options)
        : base(options) { }
}