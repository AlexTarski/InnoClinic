using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain;
using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Documents.Infrastructure.Repositories
{
    /// <summary>
    /// An abstract, generic repository class that provides basic CRUD operations for file entities.
    /// </summary>
    /// <typeparam name="T">The entity type that derives from <see cref="File"/> and represents a file in the system.</typeparam>
    public abstract class FileRepository<T> : IFileRepository<T>
        where T : File
    {
        protected readonly ILogger<FileRepository<T>> _logger;
        protected readonly DocumentsContext _context;

        protected FileRepository(DocumentsContext context, ILogger<FileRepository<T>> logger)
        {
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
            _context = context ??
                throw new DiNullReferenceException(nameof(context));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            return await _context.Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            return await _context.Set<T>()
                .FindAsync(id);
        }

        public async Task AddAsync(T file)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(AddAsync));
            await _context.AddAsync(file);
        }

        public async Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod (_logger, nameof(SaveAllAsync));
            var result = await _context.SaveChangesAsync() > 0;
            Logger.InfoBoolResult(_logger, nameof(SaveAllAsync), result ? "Success" : "Failed");

            return result;
        }
    }
}