using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;

namespace InnoClinic.Documents.Business.Interfaces
{
    public interface IFileService<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<string> GetByIdAsync(Guid id);
        Task<bool> SaveAllAsync();
    }
}