using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace InnoClinic.Documents.Business.Interfaces
{
    public interface IStorageService
    {
        public Task<string> GenerateLinkAsync(string objectPath, TimeSpan lifetime);
        public Task<string> AddFileAsync(Guid fileId, IFormFile file, UploadFileType uploadFileType);
        public Task UpdateFileAsync(IFormFile file, string objectKey);
    }
}