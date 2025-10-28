using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Profiles.Business.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorsRepository _repository;

    public DoctorService(IDoctorsRepository doctorsRepository)
    {
        _repository = doctorsRepository ?? 
                      throw new DiNullReferenceException(nameof(doctorsRepository));
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Doctor> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
            throw new KeyNotFoundException($"Doctor with ID {id} was not found");
        return result;
    }

    public async Task<bool> AddEntityAsync(Doctor model)
    {
        await _repository.AddEntityAsync(model);
        return await SaveAllAsync();
    }

    //TODO: Implement UpdateEntityAsync method
    public async Task<bool> UpdateEntityAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var doctorToDelete = await GetByIdAsync(id);
        if (doctorToDelete == null)
        {
            throw new KeyNotFoundException($"Doctor with ID {id} not found");
        }
        
        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    //TODO: Implement EntityIsValidAsync method
    public async Task<bool> EntityIsValidAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EntityExistsAsync(Guid accountId)
    {
        return await _repository.EntityExistsAsync(accountId);
    }

    public async Task<bool> IsProfileActiveAsync(Guid accountId)
    {
        var profileStatus = await _repository.GetDoctorStatusAsync(accountId);
        if (profileStatus is null)
            throw new KeyNotFoundException($"Doctor with account id {accountId} not found.");
        
        return profileStatus != DoctorStatus.Inactive;
    }

    public async Task<bool> DeactivateProfilesByOfficeIdAsync(Guid officeId)
    {
        var profilesToDeactivate = await _repository.GetAllByOfficeIdAsync(officeId);
        if (!profilesToDeactivate.Any())
        {
            throw new KeyNotFoundException($"No {nameof(Doctor)} profiles were found for this Office ID: {officeId}");
        }

        foreach (var profile in profilesToDeactivate)
        {
            profile.Status = DoctorStatus.Inactive;
            _repository.UpdateEntity(profile);
        }

        return await SaveAllAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}