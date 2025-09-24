using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Interfaces
{
    /// <summary>
    /// Defines a service contract for managing file storage.
    /// Provides asynchronous methods for retrieving, adding, and persisting files.
    /// This interface MUST be implemented by all services that interact with the storage layer.
    /// </summary>
    public interface IStorageService
    {
        public Task<string> GenerateLinkAsync(string objectPath, TimeSpan lifetime);
        public Task<string> AddFileAsync(Guid fileId, IFormFile file, UploadFileType uploadFileType);
        public Task UpdateFileAsync(IFormFile file, string objectKey);
    }
}