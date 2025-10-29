using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Pagination;

namespace InnoClinic.Profiles.Business.Interfaces;
public interface IEntityService<T, TParams>
    where T : User
    where TParams : QueryStringParameters
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<PagedList<T>> GetAllFilteredAsync(TParams queryParams);
    Task<T> GetByIdAsync(Guid id);
    Task<T> GetByAccountIdAsync(Guid accountId);
    Task<bool> AddEntityAsync(T model);
    Task<bool> UpdateEntityAsync(T model);
    Task<bool> DeleteEntityAsync(Guid id);
    Task<bool> EntityIsValidAsync(T model);
    Task<bool> EntityExistsAsync(Guid accountId);
    Task<bool> SaveAllAsync();
}