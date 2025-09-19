using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain;

public interface IReceptionistsRepository : ICrudRepository<Receptionist>
{
    public Task<IEnumerable<Receptionist>> GetAllByOfficeIdAsync(Guid officeId);
}