using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class ReceptionistsRepository : BaseCrudRepository<Receptionist>, IReceptionistsRepository
{
    public ReceptionistsRepository(ProfilesContext context, ILogger<ReceptionistsRepository> logger)
        : base(context, logger) { }

    public async Task<IEnumerable<Receptionist>> GetAllByOfficeIdAsync(Guid officeId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetAllByOfficeIdAsync));

        return await _context.Receptionists
            .Where(rec => rec.OfficeId == officeId)
            .Select(rec => rec)
            .ToListAsync();
    }
}