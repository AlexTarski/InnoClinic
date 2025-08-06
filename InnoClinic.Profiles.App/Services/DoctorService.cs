using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;

namespace InnoClinic.Profiles.Business.Services;

public class DoctorService : IDoctorService
{
    private readonly ICrudRepository<Doctor> _repository;
    private readonly IAccountService _accountService;

    public DoctorService(ICrudRepository<Doctor> crudRepository, IAccountService accountService)
    {
        _repository = crudRepository ?? 
                      throw new ArgumentNullException(nameof(crudRepository), $"{nameof(crudRepository)} must not be null");
        _accountService = accountService ?? 
                          throw new ArgumentNullException(nameof(accountService), $"{nameof(accountService)} must not be null");
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

        await _accountService.DeleteEntityAsync(doctorToDelete.AccountId);
        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    public async Task<bool> EntityIsValidAsync(Doctor model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}