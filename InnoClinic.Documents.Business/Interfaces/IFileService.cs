using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Interfaces
{
    public interface IFileService<T>
        where T : File
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<string> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(IFormFile file, UploadFileType uploadFileType);
        Task<bool> SaveAllAsync();
    }
}