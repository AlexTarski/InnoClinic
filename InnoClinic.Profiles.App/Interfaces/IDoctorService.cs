using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Business.Interfaces;

public interface IDoctorService : IEntityService<Doctor>
{
    public Task<bool> IsProfileActiveAsync(Guid accountId);
}