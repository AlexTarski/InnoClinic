using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class PatientsRepository : BaseCrudRepository<Patient>,  IPatientsRepository
{
    public PatientsRepository(ProfilesContext context)
        : base(context) { }
}