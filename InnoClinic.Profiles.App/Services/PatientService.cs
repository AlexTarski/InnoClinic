using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.App.Services;

public class PatientService : IPatientService
{
    private readonly IProfileRepository _repository;

    public PatientService(IProfileRepository repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _repository.GetAllPatientsAsync();
    }

    public async Task<Patient> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetPatientByIdAsync(id);
        if (result == null)
        {
            throw new KeyNotFoundException($"Patient with ID {id} not found");
        }
        return result;
    }

    public async Task<bool> AddEntityAsync(Patient model)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}