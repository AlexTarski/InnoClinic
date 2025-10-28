using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public class ReceptionistsRepository : BaseCrudRepository<Receptionist>, IReceptionistsRepository
{
    public ReceptionistsRepository(ProfilesContext context)
        : base(context) { }

    //TODO: review this method
    public async Task<IEnumerable<Receptionist>> GetAllByOfficeIdAsync(Guid officeId)
    {
        return await _context.Receptionists
            .Where(rec => rec.OfficeId == officeId)
            .Select(rec => rec)
            .ToListAsync();
    }
}