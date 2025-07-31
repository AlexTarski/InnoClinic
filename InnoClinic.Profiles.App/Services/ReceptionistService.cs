using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly ICrudRepository<Receptionist> _repository;

    public ReceptionistService(ICrudRepository<Receptionist> repository)
    {
        _repository = repository;
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
        throw new NotImplementedException();
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