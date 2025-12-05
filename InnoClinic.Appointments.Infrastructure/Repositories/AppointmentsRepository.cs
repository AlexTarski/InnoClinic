using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnoClinic.Appointments.Domain;
using InnoClinic.Appointments.Domain.Entities;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;
using InnoClinic.Shared.Pagination;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Appointments.Infrastructure.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly ILogger<AppointmentsRepository> _logger;
        private readonly AppointmentsContext _context;

        public AppointmentsRepository(AppointmentsContext context, ILogger<AppointmentsRepository> logger)
        {
            _logger = logger ??
                throw new DiNullReferenceException(nameof(logger));
            _context = context ??
                throw new DiNullReferenceException(nameof(context));
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            return await _context.Set<Appointment>()
                .ToListAsync();
        }

        public async Task<PagedList<Appointment>> GetAllAsync(IQueryable<Appointment> query,
            QueryStringParameters queryParams)
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
            var result = new PagedList<Appointment>(items, totalRecords, queryParams.PageNumber, queryParams.PageSize);

            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));
            return result;
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            return await _context.Set<Appointment>()
                .FindAsync(id);
        }

        public IQueryable<Appointment> GetEntityQuery()
        {
            return _context.Set<Appointment>().AsQueryable();
        }

        public async Task<bool> SaveAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(SaveAllAsync));
            return await _context.SaveChangesAsync() > 0;
        }
    }
}