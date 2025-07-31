using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class ReceptionistsRepository : BaseCrudRepository<Receptionist>
{
    public ReceptionistsRepository(ProfilesContext context)
        : base(context) { }
}