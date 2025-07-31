using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class DoctorsRepository : BaseCrudRepository<Doctor>
{
    public DoctorsRepository(ProfilesContext context)
    : base(context) { }
}