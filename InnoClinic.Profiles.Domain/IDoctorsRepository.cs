using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain;

public interface IDoctorsRepository :  ICrudRepository<Doctor>
{
    public Task<DoctorStatus?> GetDoctorStatusAsync(Guid accountId);
    public Task<IEnumerable<Doctor>> GetAllByOfficeIdAsync(Guid officeId);
}