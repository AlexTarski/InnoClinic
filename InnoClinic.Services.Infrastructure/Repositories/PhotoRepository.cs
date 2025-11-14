using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Services.Infrastructure.Repositories
{
    public class PhotoRepository : FileRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DocumentsContext context, ILogger<PhotoRepository> logger)
                : base(context, logger) { }
    }
}