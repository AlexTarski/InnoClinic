using InnoClinic.Documents.Domain.Entities;

namespace InnoClinic.Documents.Domain
{
    /// <summary>
    /// Defines a <see cref="Photo"/> repository contract for managing <see cref="Photo"/> entities.
    /// Provides asynchronous methods for retrieving, adding, and persisting file records. 
    /// </summary>
    public interface IPhotoRepository : IFileRepository<Photo>
    {
    }
}