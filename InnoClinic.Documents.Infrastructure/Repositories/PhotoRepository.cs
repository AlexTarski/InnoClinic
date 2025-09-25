using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Infrastructure.Repositories
{
    /// <summary>
    /// Defines a repository for managing <see cref="Photo"/> entities.
    /// Provides asynchronous methods for retrieving, adding, and persisting file records.
    /// </summary>
    public class PhotoRepository : FileRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DocumentsContext context, ILogger<PhotoRepository> logger)
                : base(context, logger) { }
    }
}