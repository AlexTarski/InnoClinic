namespace InnoClinic.Profiles.App.Interfaces;
public interface IEntityService<T> 
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<bool> AddEntityAsync(T model);
    Task<bool> UpdateEntityAsync(T model);
    Task<bool> DeleteEntityAsync(Guid id);
    Task<bool> EntityIsValidAsync(T model);
    Task<bool> SaveAllAsync();
}