using InnoClinic.Profiles.Domain;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public abstract class BaseCrudRepository<T> : ICrudRepository<T>
    where T : class
{
    protected readonly ProfilesContext _context;

    protected BaseCrudRepository(ProfilesContext context)
    {
        _context = context;
    }
    
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>()
            .Include(e => EF.Property<object>(e, "Account"))
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

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}