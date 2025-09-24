using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Interfaces
{
    /// <summary>
    /// Defines a generic service contract for managing <see cref="File"/> entities.
    /// Provides asynchronous methods for retrieving, adding, and persisting file records.
    /// MUST be implemented by all specific services. 
    /// </summary>
    /// <typeparam name="T">
    /// The entity type that derives from <see cref="File"/> and represents a file in the system.
    /// </typeparam>
    public interface IFileService<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<string> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(IFormFile file, UploadFileType uploadFileType);
        Task UpdateAsync(Guid fileId, IFormFile file);
        Task<bool> SaveAllAsync();
    }
}