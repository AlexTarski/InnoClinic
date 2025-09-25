using InnoClinic.Documents.Business.Interfaces;
using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Business.Services
{
    public class PhotoService : FileService<Photo>, IPhotoService
    {
        public PhotoService(IPhotoRepository repo, ILogger<PhotoService> logger, IStorageService storageService)
                : base(logger, repo, storageService) { }
    }
}