using InnoClinic.Offices.Business.Interfaces;
using InnoClinic.Offices.Domain;
using InnoClinic.Shared.Exceptions;

namespace InnoClinic.Offices.Business.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly IOfficesRepository _repository;

        public OfficeService(IOfficesRepository officesRepository)
        {
            _repository = officesRepository ??
                          throw new DiNullReferenceException(nameof(officesRepository));
        }

        public async Task<IEnumerable<Office>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize)
        {
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than zero.");
            }
            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
            }

            return await _repository.GetAllAsync(page, pageSize);
        }

        public async Task<Office> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result ?? throw new KeyNotFoundException($"Office with ID {id} was not found");
        }
    }
}