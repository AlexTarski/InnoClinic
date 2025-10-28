using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Infrastructure.Repositories;

public abstract class BaseCrudRepository<T> : ICrudRepository<T>
    where T : User
{
    protected readonly ILogger<BaseCrudRepository<T>> _logger;
    protected readonly ProfilesContext _context;

    protected BaseCrudRepository(ProfilesContext context, ILogger<BaseCrudRepository<T>> logger)
    {
        _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
        _context = context ?? throw new DiNullReferenceException(nameof(context));
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
        var result = await _context.Set<T>()
            .ToListAsync();
        Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

        return result;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
        var result = await _context.Set<T>()
            .FindAsync(id);
        Logger.DebugExitingMethod(_logger, nameof(GetByIdAsync));

        return result!;
    }

    public async Task<T> GetByAccountIdAsync(Guid accountId)
    {
        Logger.DebugStartProcessingMethod(_logger, nameof(GetByAccountIdAsync));
        var result = await _context.Set<T>()
            .FirstOrDefaultAsync(user => user.AccountId == accountId);
        Logger.DebugExitingMethod(_logger, nameof(GetByAccountIdAsync));

        return result!;
    }

    //TODO: review this method
    public async Task AddEntityAsync(T model)
    {
        await _context.AddAsync(model);
    }

    //TODO: review this method
    public void UpdateEntity(T model)
    {
        _context.Update(model);
    }

    //TODO: review this method
    public async Task DeleteEntityAsync(Guid id)
    {
        _context.Remove(await GetByIdAsync(id));
    }

    //TODO: review this method
    public async Task<bool> EntityExistsAsync(Guid accountId)
    {
        return await _context.Set<T>().AnyAsync(entity => entity.AccountId == accountId);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}