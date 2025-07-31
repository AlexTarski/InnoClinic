using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Domain
{
    public interface ICrudRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddEntityAsync(T model);
        void UpdateEntity(T model);
        void DeleteEntity(Guid id);
        Task<bool> SaveAllAsync();
    }
}
