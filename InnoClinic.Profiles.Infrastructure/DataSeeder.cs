using InnoClinic.Profiles.Domain.Entities.Users;
using InnoClinic.Shared.DataSeeding;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure
{
    public class DataSeeder
    {
        private readonly ProfilesContext _context;

        public DataSeeder(ProfilesContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!await _context.Doctors.AnyAsync())
            {
                var doctors = new List<Doctor>()
                {
                    CreateDoctor(SampleData.ElenaVolkova),
                    CreateDoctor(SampleData.SergeyIvanov),
                    CreateDoctor(SampleData.AminaSadikova)
                };

                await _context.Doctors.AddRangeAsync(doctors);
            }

            if (!await _context.Receptionists.AnyAsync())
            {
                var receptionists = new List<Receptionist>()
                {
                    CreateReceptionist(SampleData.OlgaSmirnova),
                    CreateReceptionist(SampleData.MateuszKowalski),
                    CreateReceptionist(SampleData.LeylaAbdulova)
                };
                
                await _context.Receptionists.AddRangeAsync(receptionists);
            }

            if (!await _context.Patients.AnyAsync())
            {
                var patients = new List<Patient>()
                {
                    CreatePatient(SampleData.MaximPetrov),
                    CreatePatient(SampleData.CharlotteBergman),
                    CreatePatient(SampleData.RajeshMehta),
                };
                
                await _context.Patients.AddRangeAsync(patients);
            }
            
            await _context.SaveChangesAsync();
        }

        private static Doctor CreateDoctor(Guid accountId)
        {
            var accountData = SampleData.Doctors[accountId];
            return new Doctor
            {
                Id = accountData.Profile.Id,
                FirstName = accountData.Profile.FirstName,
                LastName = accountData.Profile.LastName,
                MiddleName = accountData.Profile.MiddleName,
                AccountId = accountId,
                DateOfBirth = accountData.Profile.DateOfBirth,
                SpecializationId = accountData.Profile.SpecializationId,
                OfficeId = accountData.Profile.OfficeId,
                CareerStartYear = accountData.Profile.CareerStartYear,
                Status = (DoctorStatus)accountData.Profile.Status
            };
        }

        private static Receptionist CreateReceptionist(Guid accountId)
        {
            var accountData = SampleData.Receptionists[accountId];
            return new Receptionist
            {
                Id = accountData.Profile.Id,
                FirstName = accountData.Profile.FirstName,
                LastName = accountData.Profile.LastName,
                MiddleName = accountData.Profile.MiddleName,
                AccountId = accountId,
                OfficeId = accountData.Profile.OfficeId
            };
        }

        private static Patient CreatePatient(Guid accountId)
        {
            var accountData = SampleData.Patients[accountId];
            return new Patient
            {
                Id = accountData.Profile.Id,
                FirstName = accountData.Profile.FirstName,
                LastName = accountData.Profile.LastName,
                MiddleName = accountData.Profile.MiddleName,
                AccountId = accountId,
                DateOfBirth = accountData.Profile.DateOfBirth,
                IsLinkedToAccount = true
            };
        }
    }
}