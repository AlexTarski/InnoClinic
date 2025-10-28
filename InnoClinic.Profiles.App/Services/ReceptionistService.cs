using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Profiles.Business.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IReceptionistsRepository _repository;

    public ReceptionistService(IReceptionistsRepository repository)
    {
        _repository = repository ?? 
                      throw new DiNullReferenceException(nameof(repository));
    }
    
    public async Task<IEnumerable<Receptionist>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Receptionist> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
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

    //TODO: Implement UpdateEntityAsync method
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

        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    //TODO: Implement EntityIsValidAsync method
    public async Task<bool> EntityIsValidAsync(Receptionist model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EntityExistsAsync(Guid accountId)
    {
        return await _repository.EntityExistsAsync(accountId);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}