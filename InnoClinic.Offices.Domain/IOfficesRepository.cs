namespace InnoClinic.Offices.Domain
{
    public interface IOfficesRepository
    {
        Task<IEnumerable<Office>> GetAllAsync();
        Task<Office> GetByIdAsync(Guid id);
    }
}