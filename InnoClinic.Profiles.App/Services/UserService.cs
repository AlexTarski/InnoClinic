using InnoClinic.Profiles.Business.Interfaces;
using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Profiles.Business.Services
{
    public abstract class UserService<T, TParams> : IEntityService<T, TParams>
        where T : User
        where TParams : QueryStringParameters
    {
        protected readonly ILogger<UserService<T, TParams>> _logger;
        protected readonly ICrudRepository<T> _repository;

        protected UserService(ICrudRepository<T> repository, ILogger<UserService<T, TParams>> logger)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            var result = await _repository.GetAllAsync();
            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

            return result;
        }

        public abstract Task<PagedList<T>> GetAllFilteredAsync(TParams queryParams);

        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var result = await _repository.GetByIdAsync(id);
            Logger.DebugExitingMethod(_logger, nameof(GetByIdAsync));
                
            return result ?? throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} was not found");
        }

        public async Task<T> GetByAccountIdAsync(Guid accountId)
        {
            Logger.DebugStartProcessingMethod (_logger, nameof(GetByAccountIdAsync));
            var result = await _repository.GetByAccountIdAsync(accountId);
            Logger.DebugExitingMethod (_logger, nameof(GetByAccountIdAsync));

            return result ?? throw new KeyNotFoundException($"{typeof(T).Name} with account ID {accountId} was not found");
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