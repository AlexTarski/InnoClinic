using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

using Amazon.S3;

using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Business.Services
{
    /// <summary>
    /// An abstract, generic service class that provides basic CRUD operations for file entities.
    /// </summary>
    /// <typeparam name="T">The entity type that derives from <see cref="File"/> and represents a file in the system.</typeparam>
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

        //TODO: Refactor for Document upload
        public async Task<Guid> AddAsync(IFormFile file, UploadFileType uploadFileType)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(AddAsync));
            var fileId = Guid.NewGuid();
            try
            {
                var objectKey = await _storageService.AddFileAsync(fileId, file, uploadFileType);
                var newEntity = CreatePhoto(fileId, objectKey);
                await _repository.AddAsync(newEntity);
                await SaveAllAsync();
                Logger.DebugExitingMethod(_logger, nameof(AddAsync));

                return fileId;
            }
            catch (InvalidEnumArgumentException ex)
            {
                Logger.Error(_logger, ex, $"File uploading failed: invalid {nameof(UploadFileType)}");
                throw;
            }
            catch (UploadFailedException ex)
            {
                Logger.Error(_logger, ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Guid fileId, IFormFile file)
        {
            Logger.DebugStartProcessingMethod (_logger, nameof(UpdateAsync));
            var fileData = await _repository.GetByIdAsync(fileId) 
                ?? throw new KeyNotFoundException($"{typeof(T).Name} with ID {fileId} was not found");
            try
            {
                await _storageService.UpdateFileAsync(file, fileData.Url);
                Logger.DebugExitingMethod(_logger, nameof(UpdateAsync));
            }
            catch (UploadFailedException ex)
            {
                Logger.Error (_logger, ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SaveAllAsync));
            return await _repository.SaveAllAsync();
        }

        private T CreatePhoto(Guid photoId, string photoUrl)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(CreatePhoto));
            var newPhoto = Activator.CreateInstance<T>();
            newPhoto.Id = photoId;
            newPhoto.Url = photoUrl;
            Logger.DebugExitingMethod(_logger, nameof(CreatePhoto));

            return newPhoto;
        }
    }
}