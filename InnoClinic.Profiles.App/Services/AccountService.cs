using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Services;

public class AccountService : IAccountService
{
    private readonly ICrudRepository<Account> _repository;

    public AccountService(ICrudRepository<Account> crudRepository)
    {
        _repository = crudRepository;
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Account> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
            throw new KeyNotFoundException($"{nameof(Account)} with ID {id} was not found");
        return result;
    }

    public async Task<bool> AddEntityAsync(Account model)
    {
        await _repository.AddEntityAsync(model);
        return await SaveAllAsync();
    }

    public async Task<bool> UpdateEntityAsync(Account model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var accountToDelete = await GetByIdAsync(id);
        if (accountToDelete == null)
        {
            throw new KeyNotFoundException($"Account with ID {id} not found");
        }

        await _repository.DeleteEntityAsync(id);
        return true;
    }

    public async Task<bool> EntityIsValidAsync(Account model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}