using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Domain;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Offices.Business.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficesRepository _repository;
        private readonly ILogger<OfficeService> _logger;

        public OfficeService(IOfficesRepository officesRepository, ILogger<OfficeService> logger)
        {
            _logger = logger ??
                          throw new DiNullReferenceException(nameof(logger));
            _repository = officesRepository ??
                          throw new DiNullReferenceException(nameof(officesRepository));
        }

        public async Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            if (page == 0 && pageSize == 0)
            {
                Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));
                return await _repository.GetAllAsync();
            }
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than zero.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
            }

            var result = await _repository.GetAllAsync(page, pageSize);
            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

            return result;
        }

        public async Task<Office> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var result = await _repository.GetByIdAsync(id);
            Logger.DebugExitingMethod(_logger, nameof(GetByIdAsync));

            return result ?? throw new KeyNotFoundException($"Office with ID {id} was not found");
        }

        public async Task<bool> AddAsync(Office newOffice)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(AddAsync));

            var existingOffice = await _repository.GetByIdAsync(newOffice.Id);
            if (existingOffice != null)
            {
                throw new InvalidOperationException($"{nameof(Office)} with ID {newOffice.Id} already exists.");
            }

            await _repository.AddAsync(newOffice);
            var result = await _repository.SaveAllAsync();
            Logger.InfoBoolResult(_logger, nameof(AddAsync), result.ToString());
            Logger.DebugExitingMethod(_logger, nameof(AddAsync));

            return result;
        }
    }
}