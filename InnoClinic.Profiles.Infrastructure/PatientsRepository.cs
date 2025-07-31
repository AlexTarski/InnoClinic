using InnoClinic.Profiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class PatientsRepository : BaseCrudRepository<Patient>
{
    public PatientsRepository(ProfilesContext context) 
        : base(context) { }
}