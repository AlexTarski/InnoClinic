using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class ReceptionistsRepository : BaseCrudRepository<Receptionist>, IReceptionistsRepository
{
    public ReceptionistsRepository(ProfilesContext context)
        : base(context) { }
}