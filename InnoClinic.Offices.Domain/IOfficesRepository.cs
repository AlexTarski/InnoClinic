namespace InnoClinic.Offices.Domain
{
    public interface IOfficesRepository
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<Office> GetByIdAsync(Guid id);
        Task AddAsync(Office model);
        void Update(Office model);
        Task DeleteAsync(Guid id);
        Task<bool> SaveAllAsync();
    }
}