using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Interfaces
{
    public interface IOfficeService
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<Office> GetByIdAsync(Guid id);
    }
}