using InnoClinic.Profiles.Business.Filters;
using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services;

public class ReceptionistService : UserService<Receptionist, ReceptionistParameters>, IReceptionistService
{
    public ReceptionistService(IReceptionistsRepository repository, ILogger<ReceptionistService> logger)
        : base(repository, logger) {}

    public override void ApplyFilters(ref IQueryable<Receptionist> query, ReceptionistParameters queryParams)
    {
    }
}