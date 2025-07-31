using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class PatientsRepository : BaseCrudRepository<Patient>
{
    public PatientsRepository(ProfilesContext context)
        : base(context) { }
}