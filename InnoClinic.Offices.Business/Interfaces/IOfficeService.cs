using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Interfaces
{
    public interface IOfficeService
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize);
        Task<Office> GetByIdAsync(Guid id);
    }
}