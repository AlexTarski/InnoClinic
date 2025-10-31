using InnoClinic.Profiles.Business.Filters;
using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Business.Interfaces;

public interface IDoctorService : IEntityService<Doctor, DoctorParameters>
{
    public Task<bool> IsProfileActiveAsync(Guid accountId);
    public Task<bool> DeactivateProfilesByOfficeIdAsync(Guid officeId);
}