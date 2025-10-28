using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class PatientsRepository : BaseCrudRepository<Patient>,  IPatientsRepository
{
    public PatientsRepository(ProfilesContext context, ILogger<PatientsRepository> logger)
        : base(context, logger) { }
}