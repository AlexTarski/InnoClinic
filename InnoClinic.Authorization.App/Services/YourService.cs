using InnoClinic.Authorization.Business.Interfaces;
using InnoClinic.Authorization.Domain;
using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Business.Services;

public class YourService : IYourEntityService
{
    private readonly ICrudRepository<YourEntity> _repository;

    public YourService(ICrudRepository<YourEntity> crudRepository)
    {
        _repository = crudRepository;
    }

    public async Task<IEnumerable<YourEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<YourEntity> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
            throw new KeyNotFoundException($"{typeof(YourEntity).Name} with ID {id} was not found");
        return result;
    }

    public async Task<bool> AddEntityAsync(YourEntity model)
    {
        await _repository.AddEntityAsync(model);
        return await SaveAllAsync();
    }

    public async Task<bool> UpdateEntityAsync(YourEntity model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteEntityAsync(Guid id)
    {
        var doctorToDelete = await GetByIdAsync(id);
        if (doctorToDelete == null)
        {
            throw new KeyNotFoundException($"{typeof(YourEntity).Name} with ID {id} not found");
        }

        await _repository.DeleteEntityAsync(id);
        return await SaveAllAsync();
    }

    public async Task<bool> EntityIsValidAsync(YourEntity model)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _repository.SaveAllAsync();
    }
}