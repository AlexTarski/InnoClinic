using InnoClinic.Profiles.App.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.App.Services;

public class DoctorService : IDoctorService
{
    private readonly IProfileRepository _repository;

    public DoctorService(IProfileRepository profileRepository)
    {
        _repository = profileRepository;
    }
    
    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _repository.GetAllDoctorsAsync();
    }

    public async Task<Doctor> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetDoctorByIdAsync(id);
        if(result == null)
            throw new KeyNotFoundException($"Doctor with ID {id} was not found");
        return result;
    }

    public async Task<bool> AddEntityAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateEntityAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EntityIsValidAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        throw new NotImplementedException();
    }
}