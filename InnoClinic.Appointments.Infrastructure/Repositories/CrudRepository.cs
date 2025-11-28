using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InnoClinic.Services.Domain;
using InnoClinic.Services.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Infrastructure.Repositories
{
    public abstract class CrudRepository<T> : ICrudRepository<T>
        where T : Entity
    {
        protected readonly ILogger<CrudRepository<T>> _logger;
        protected readonly ServicesContext _context;

        protected CrudRepository(ServicesContext context, ILogger<CrudRepository<T>> logger)
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

        public virtual async Task<PagedList<T>> GetAllAsync(IQueryable<T> query, QueryStringParameters queryParams)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            var totalRecords = await query.CountAsync();
            Logger.Information(_logger, $"Total count of records: {totalRecords}");

            long skipCount = ((long)queryParams.PageNumber - 1) * queryParams.PageSize;

            if (skipCount > int.MaxValue)
                throw new OverflowException("Skip value exceeds Int32.MaxValue.");

            Logger.InfoTryDoAction(_logger, "Retrieving paginated data");
            var items = await query.Skip((int)skipCount)
                                   .Take(queryParams.PageSize)
                                   .ToListAsync();

            Logger.InfoTryDoAction(_logger, "Returning paginated data");
            var result = new PagedList<T>(items, totalRecords, queryParams.PageNumber, queryParams.PageSize);

            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));
            return result;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            return await _context.Set<T>()
                .FindAsync(id);
        }

        public IQueryable<T> GetEntityQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SaveAllAsync));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}