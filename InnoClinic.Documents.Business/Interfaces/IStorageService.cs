using System;
using System.Threading.Tasks;

namespace InnoClinic.Documents.Business.Interfaces
{
    public interface IStorageService
    {
        public Task<string> GenerateLinkAsync(string objectPath, TimeSpan lifetime);
    }
}