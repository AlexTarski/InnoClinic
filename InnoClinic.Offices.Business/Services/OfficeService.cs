using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficesRepository _repository;

        public OfficeService(IOfficesRepository officesRepository)
        {
            _repository = officesRepository ??
                          throw new ArgumentNullException(nameof(officesRepository), $"{nameof(officesRepository)} must not be null");
        }

        public Task<bool> AddAsync(Office model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
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

        public Task<bool> UpdateAsync(Office model)
        {
            throw new NotImplementedException();
        }
    }
}