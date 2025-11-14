using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Services.Business.Interfaces;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Business.Services
{
    public class PhotoService : FileService<Photo>, IPhotoService
    {
        public PhotoService(IPhotoRepository repo, ILogger<PhotoService> logger)
                : base(logger, repo) { }
    }
}