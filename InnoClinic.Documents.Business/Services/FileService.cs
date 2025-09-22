using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Business.Services
{
    public abstract class FileService<T> : IFileService<T>
        where T : File
    {
        protected readonly ILogger<FileService<T>> _logger;
        protected readonly IFileRepository<T> _repository;

        protected FileService(ILogger<FileService<T>> logger, IFileRepository<T> repository)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            return _repository.GetAllAsync();
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SaveAllAsync));
            return _repository.SaveAllAsync();
        }
    }
}