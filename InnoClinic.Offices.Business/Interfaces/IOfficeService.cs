using InnoClinic.Offices.Domain;

namespace InnoClinic.Offices.Business.Interfaces
{
    public interface IOfficeService
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<Office> GetByIdAsync(Guid id);
        Task<bool> AddAsync(Office model);
        Task<bool> UpdateAsync(Office model);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SaveAllAsync();
    }
}