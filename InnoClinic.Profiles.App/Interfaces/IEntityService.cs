using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Business.Interfaces;
public interface IEntityService<T>
    where T : User
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByAccountIdAsync(Guid accountId);
    Task<bool> AddEntityAsync(T model);
    Task<bool> UpdateEntityAsync(T model);
    Task<bool> DeleteEntityAsync(Guid id);
    Task<bool> EntityIsValidAsync(T model);
    Task<bool> EntityExistsAsync(Guid accountId);
    Task<bool> SaveAllAsync();
}