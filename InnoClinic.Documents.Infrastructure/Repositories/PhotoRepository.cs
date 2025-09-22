using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Infrastructure.Repositories
{
    public class PhotoRepository : FileRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DocumentsContext context, ILogger<PhotoRepository> logger)
                : base(context, logger) { }
    }
}