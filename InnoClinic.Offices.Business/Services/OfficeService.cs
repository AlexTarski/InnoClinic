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

        public async Task<Office> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result ?? throw new KeyNotFoundException($"Office with ID {id} was not found");
        }
    }
}