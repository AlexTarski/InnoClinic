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
                          throw new DiNullReferenceException(nameof(_repository));
        }

        public Task<IEnumerable<Office>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Office> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}