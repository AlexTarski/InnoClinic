using InnoClinic.Offices.Domain;
using InnoClinic.Shared;
using InnoClinic.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InnoClinic.Offices.Infrastructure
{
    public class OfficesRepository : IOfficesRepository
    {
        private readonly ILogger<OfficesRepository> _logger;
        private readonly OfficesDbContext _context;

        public OfficesRepository(OfficesDbContext context, ILogger<OfficesRepository> logger)
        {
            _logger = logger ??
                        throw new DiNullReferenceException(nameof(logger));
            _context = context ?? 
                        throw new DiNullReferenceException(nameof(context));
        }

        public async Task<IEnumerable<Office>> GetAllAsync()
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            var result = await _context.Offices.ToListAsync();
            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

            return result;
        }

        public async Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetAllAsync));
            var result = await _context.Offices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            Logger.DebugExitingMethod(_logger, nameof(GetAllAsync));

            return result;
        }

        public async Task<Office> GetByIdAsync(Guid id)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(GetByIdAsync));
            var result = await _context.Offices.FindAsync(id);

            return result;
        }

        public async Task AddAsync(Office newOffice)
        {
            Logger.DebugStartProcessingMethod(_logger, nameof(AddAsync));
            await _context.Offices.AddAsync(newOffice);
            Logger.DebugExitingMethod(_logger, nameof(AddAsync));
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}