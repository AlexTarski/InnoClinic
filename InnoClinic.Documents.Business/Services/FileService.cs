using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Amazon.S3;

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
        protected readonly IStorageService _storageService;

        protected FileService(ILogger<FileService<T>> logger, IFileRepository<T> repository, IStorageService storageService)
        {
            _logger = logger ?? throw new DiNullReferenceException(nameof(logger));
            _repository = repository ?? throw new DiNullReferenceException(nameof(repository));
            _storageService = storageService ?? throw new DiNullReferenceException(nameof(storageService));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            return await _repository.GetAllAsync();
        }

        public async Task<string> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var file = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} was not found");
            try
            {
                return await _storageService.GenerateLinkAsync(file.Url, new TimeSpan(0, 5, 0));
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Logger.WarningFailedDoAction(_logger, nameof(GetByIdAsync));
                throw new KeyNotFoundException($"{typeof(T).Name} with ID {id} was not found", ex);
            }
            catch (Exception)
            {
                Logger.WarningFailedDoAction(_logger, nameof(GetByIdAsync));
                throw;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SaveAllAsync));
            return await _repository.SaveAllAsync();
        }
    }
}