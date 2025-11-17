using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Services.Business.Interfaces;
using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Business.Services
{
    public abstract class EntityService<T, TParams> : IEntityService<T, TParams>
    where T : Entity
    where TParams : QueryStringParameters
    {
        protected readonly ILogger<EntityService<T, TParams>> _logger;
        protected readonly ICrudRepository<T> _repository;

        protected EntityService(ICrudRepository<T> repository, ILogger<EntityService<T, TParams>> logger)
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

        public async Task<PagedList<T>> GetAllFilteredAsync(TParams queryParams)
        {
            try
            {
                Logger.DebugStartProcessingMethod(_logger, nameof(GetAllFilteredAsync));
                var query = _repository.GetEntityQuery();
                ApplyFilters(ref query, queryParams);
                query = query.OrderBy(user => user.Id);

                var result = await _repository.GetAllAsync(query, queryParams);
                Logger.DebugExitingMethod(_logger, nameof(GetAllFilteredAsync));

                return result;
            }
            catch (Exception ex) when (ex is OverflowException || ex is PageOutOfRangeException)
            {
                Logger.WarningFailedDoAction(_logger, nameof(GetAllFilteredAsync));

                throw new PaginationArgumentException($"Failed to get {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var result = await _repository.GetByIdAsync(id);
            Logger.DebugExitingMethod(_logger, nameof(GetByIdAsync));

            return result ?? throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} was not found");
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _repository.SaveAllAsync();
        }

        public abstract void ApplyFilters(ref IQueryable<T> query, TParams queryParams);
    }
}