using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Infrastructure
{
    public class OfficesRepository : IOfficesRepository
    {
        private readonly OfficesDbContext _context;

        public OfficesRepository(OfficesDbContext context)
        {
            _context = context ?? 
                       throw new ArgumentNullException(nameof(context),  $"{nameof(context)} must not be null");
        }

        public Task AddAsync(Office model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Office>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Office> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Office model)
        {
            throw new NotImplementedException();
        }
    }
}