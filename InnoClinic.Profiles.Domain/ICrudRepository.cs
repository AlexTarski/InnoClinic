using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Domain
{
    public interface ICrudRepository<T>
        where T : User
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddEntityAsync(T model);
        void UpdateEntity(T model);
        Task DeleteEntityAsync(Guid id);
        Task<bool> EntityExistsAsync(Guid accountId);
        Task<bool> SaveAllAsync();
    }
}