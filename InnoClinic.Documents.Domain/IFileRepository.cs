using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;

namespace InnoClinic.Documents.Domain
{
    /// <summary>
    /// Defines a generic repository contract for managing <see cref="File"/> entities.
    /// Provides asynchronous methods for retrieving, adding, and persisting file records.
    /// MUST be implemented by all specific repositories. 
    /// </summary>
    /// <typeparam name="T">
    /// The entity type that derives from <see cref="File"/> and represents a file in the system.
    /// </typeparam>
    public interface IFileRepository<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T file);
        Task<bool> SaveAllAsync();
    }
}