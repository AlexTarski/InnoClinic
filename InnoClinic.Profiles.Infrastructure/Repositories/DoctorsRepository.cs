using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class DoctorsRepository : BaseCrudRepository<Doctor>, IDoctorsRepository
{
    public DoctorsRepository(ProfilesContext context, ILogger<DoctorsRepository> logger)
    : base(context, logger) { }

    public async Task<DoctorStatus?> GetDoctorStatusAsync(Guid accountId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetDoctorStatusAsync));
        return await _context.Doctors
            .Where(doc => doc.AccountId == accountId)
            .Select(doc => (DoctorStatus?)doc.Status)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Doctor>> GetAllByOfficeIdAsync(Guid officeId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetAllByOfficeIdAsync));
        return await _context.Doctors
            .Where(doc => doc.OfficeId == officeId)
            .Select(doc => doc)
            .ToListAsync();
    }
}