namespace InnoClinic.Offices.Domain
{
    public interface IOfficesRepository
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<IEnumerable<Office>> GetAllAsync(int page, int pageSize);
        Task<Office> GetByIdAsync(Guid id);
        Task AddAsync(Office newOffice);
        void Update(Office updatedOffice);
        Task<bool> OfficeExistsAsync(Office office);
        Task<bool> OfficeAddressExistsAsync(Address address);
        Task<bool> SaveAllAsync();
    }
}