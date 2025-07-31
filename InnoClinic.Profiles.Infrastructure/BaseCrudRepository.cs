using InnoClinic.Profiles.Domain;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public abstract class BaseCrudRepository<T> : ICrudRepository<T>
    where T : class
{
    private readonly ProfilesContext _context;

    protected BaseCrudRepository(ProfilesContext context)
    {
        _context = context;
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

    public void DeleteEntity(Guid id)
    {
        _context.Remove(GetByIdAsync(id));
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}