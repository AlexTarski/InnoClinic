using InnoClinic.Offices.Domain;
using InnoClinic.Shared.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Offices.Infrastructure
{
    public class OfficesRepository : IOfficesRepository
    {
        private readonly OfficesDbContext _context;

        public OfficesRepository(OfficesDbContext context)
        {
            _context = context ?? 
                       throw new DiNullReferenceException(nameof(context));
        }

        public async Task<IEnumerable<Office>> GetAllAsync()
        {
            return await _context.Offices.ToListAsync();
        }

        public async Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize)
        {
            return await _context.Offices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Office> GetByIdAsync(Guid id)
        {
            return await _context.Offices.FindAsync(id);
        }
    }
}