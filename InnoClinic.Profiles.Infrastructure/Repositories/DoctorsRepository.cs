using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class DoctorsRepository : BaseCrudRepository<Doctor>, IDoctorsRepository
{
    public DoctorsRepository(ProfilesContext context)
    : base(context) { }

    public async Task<DoctorStatus?> GetDoctorStatusAsync(Guid accountId)
    {
        return await _context.Doctors
            .Where(doc => doc.AccountId == accountId)
            .Select(doc => (DoctorStatus?)doc.Status)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Doctor>> GetAllByOfficeIdAsync(Guid officeId)
    {
        return await _context.Doctors
            .Where(doc => doc.OfficeId == officeId)
            .Select(doc => doc)
            .ToListAsync();
    }
}