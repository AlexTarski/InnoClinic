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
                    CreateDoctor(Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381")),
                    CreateDoctor(Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397")),
                    CreateDoctor(Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"))
                };

                await _context.Doctors.AddRangeAsync(doctors);
            }

            if (!await _context.Receptionists.AnyAsync())
            {
                var receptionists = new List<Receptionist>()
                {
                    CreateReceptionist(Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5")),
                    CreateReceptionist(Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6")),
                    CreateReceptionist(Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7"))
                };
                
                await _context.Receptionists.AddRangeAsync(receptionists);
            }

            if (!await _context.Patients.AnyAsync())
            {
                var patients = new List<Patient>()
                {
                    CreatePatient(Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107")),
                    CreatePatient(Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f")),
                    CreatePatient(Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2")),
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