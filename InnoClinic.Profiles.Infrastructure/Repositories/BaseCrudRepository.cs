using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public abstract class BaseCrudRepository<T> : ICrudRepository<T>
    where T : User
{
    protected readonly ProfilesContext _context;

    protected BaseCrudRepository(ProfilesContext context)
    {
        _context = context ?? 
                   throw new ArgumentNullException(nameof(context),  $"{nameof(context)} must not be null");
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>()
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>()
            .FindAsync(id);
    }

    public async Task AddEntityAsync(T model)
    {
        await _context.AddAsync(model);
    }

    public void UpdateEntity(T model)
    {
        _context.Update(model);
    }

    public async Task DeleteEntityAsync(Guid id)
    {
        _context.Remove(await GetByIdAsync(id));
    }
    
    public async Task<bool> EntityExistsAsync(Guid accountId)
    {
        return await _context.Set<T>().AnyAsync(entity => entity.AccountId == accountId);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}