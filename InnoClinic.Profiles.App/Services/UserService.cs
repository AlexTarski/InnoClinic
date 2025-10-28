using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services
{
    public abstract class UserService<T> : IEntityService<T>
        where T : User
    {
        protected readonly ILogger<UserService<T>> _logger;
        protected readonly ICrudRepository<T> _repository;

        protected UserService(ICrudRepository<T> repository, ILogger<UserService<T>> logger)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            return await _repository.GetAllAsync();
        }

        //TODO: review this method
        public async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null)
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} was not found");
            return result;
        }

        //TODO: review this method
        public async Task<bool> AddEntityAsync(T model)
        {
            await _repository.AddEntityAsync(model);
            return await SaveAllAsync();
        }

        //TODO: implement UpdateEntityAsync method
        public Task<bool> UpdateEntityAsync(T model)
        {
            throw new NotImplementedException();
        }

        //TODO: review this method
        public async Task<bool> DeleteEntityAsync(Guid id)
        {
            var userToDelete = await GetByIdAsync(id);
            if (userToDelete == null)
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} not found");
            }

            await _repository.DeleteEntityAsync(id);
            return await SaveAllAsync();
        }

        public async Task<bool> EntityExistsAsync(Guid accountId)
        {
            return await _repository.EntityExistsAsync(accountId);
        }

        //TODO: Implement EntityIsValidAsync method
        public Task<bool> EntityIsValidAsync(T model)
        {
            throw new NotImplementedException();
        }

        //TODO: review this method
        public async Task<bool> SaveAllAsync()
        {
            return await _repository.SaveAllAsync();
        }
    }
}