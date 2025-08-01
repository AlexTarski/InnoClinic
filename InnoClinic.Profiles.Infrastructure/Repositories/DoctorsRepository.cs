using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class DoctorsRepository : BaseCrudRepository<Doctor>
{
    public DoctorsRepository(ProfilesContext context)
    : base(context) { }
}