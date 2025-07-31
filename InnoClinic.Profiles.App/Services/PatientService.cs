using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Business.Services;

public class PatientService : IPatientService
{
    private readonly ICrudRepository<Patient> _repository;

    public PatientService(ICrudRepository<Patient> repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Patient> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Patient with ID {id} not found");
        }
        return result;
    }

    public async Task<bool> AddEntityAsync(Patient model)
    {
        await _repository.AddEntityAsync(model);
        return await SaveAllAsync();
    }

    public async Task<bool> UpdateEntityAsync(Patient model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EntityIsValidAsync(Patient model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}