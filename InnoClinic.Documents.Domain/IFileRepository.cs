using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;

namespace InnoClinic.Documents.Domain
{
    public interface IFileRepository<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T file);
        Task<bool> SaveAllAsync();
    }
}