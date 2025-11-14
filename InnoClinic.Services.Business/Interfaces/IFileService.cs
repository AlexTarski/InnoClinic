using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Services.Domain.Entities;

namespace InnoClinic.Services.Business.Interfaces
{
    public interface IFileService<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<bool> SaveAllAsync();
    }
}