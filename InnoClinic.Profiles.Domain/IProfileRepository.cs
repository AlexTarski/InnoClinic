using InnoClinic.Profiles.Domain.Entities;

namespace InnoClinic.Profiles.Domain
{
    public interface IProfileRepository
    {
        Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<IEnumerable<Receptionist>> GetAllReceptionistsAsync();
        Task<Doctor> GetDoctorByIdAsync(Guid id);
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<Receptionist> GetReceptionistByIdAsync(Guid id);
        Task AddEntityAsync(Object model);
        void UpdateEntity(Object model);
        void DeleteEntity(Guid id);
        Task<bool> SaveAllAsync();
    }
}
