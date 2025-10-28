using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Profiles.Business.Services;

public class PatientService : IPatientService
{
    private readonly IPatientsRepository _repository;

    public PatientService(IPatientsRepository repository)
    {
        _repository = repository ?? 
                      throw new DiNullReferenceException(nameof(repository));
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

    //TODO: Implement UpdateEntityAsync method
    public async Task<bool> UpdateEntityAsync(Patient model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var patientToDelete = await GetByIdAsync(id);
        if (patientToDelete == null)
        {
            throw new KeyNotFoundException($"Patient with ID {id} not found");
        }
        
        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    //TODO: Implement EntityIsValidAsync method
    public async Task<bool> EntityIsValidAsync(Patient model)
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