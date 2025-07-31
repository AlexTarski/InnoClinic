using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly ICrudRepository<Receptionist> _repository;
    private readonly IAccountService _accountService;

    public ReceptionistService(ICrudRepository<Receptionist> repository, IAccountService accountService)
    {
        _repository = repository;
        _accountService = accountService;
    }
    public async Task<IEnumerable<Receptionist>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Receptionist> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if(result == null)
        {
            throw new KeyNotFoundException($"Receptionist with ID {id} not found");
        }
        return result;
    }

    public async Task<bool> AddEntityAsync(Receptionist model)
    {
        await _repository.AddEntityAsync(model);
        return await SaveAllAsync();
    }

    public async Task<bool> UpdateEntityAsync(Receptionist model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var receptionistToDelete = await GetByIdAsync(id);
        if (receptionistToDelete == null)
        {
            throw new KeyNotFoundException($"Receptionist with ID {id} not found");
        }
        await _accountService.DeleteEntityAsync(receptionistToDelete.AccountId);
        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    public async Task<bool> EntityIsValidAsync(Receptionist model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}