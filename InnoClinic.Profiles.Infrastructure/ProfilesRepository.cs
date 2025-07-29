using InnoClinic.Profiles.Domain;
using InnoClinic.Profiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class ProfilesRepository : IProfileRepository
{
    private readonly ProfilesContext _context;

    public ProfilesRepository(ProfilesContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await _context.Patients
            .ToListAsync();
    }

    public async Task<IEnumerable<Receptionist>> GetAllReceptionistsAsync()
    {
        return await _context.Receptionists
            .ToListAsync();
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid id)
    {
        return await _context.Doctors
            .FirstOrDefaultAsync(doctor => doctor.Id == id);
    }

    public async Task<Patient> GetPatientByIdAsync(Guid id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(patient => patient.Id == id);
    }

    public async Task<Receptionist> GetReceptionistByIdAsync(Guid id)
    {
        return await _context.Receptionists
            .FirstOrDefaultAsync(receptionist => receptionist.Id == id);
    }

    public async Task AddEntityAsync(object model)
    {
        await _context.AddAsync(model);
    }

    public void UpdateEntity(object model)
    {
        _context.Update(model);
    }

    public void DeleteEntity(Guid id)
    {
        _context.Remove(
            _context.Doctors
                .FirstOrDefault(doctor => doctor.Id == id)
            );
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}