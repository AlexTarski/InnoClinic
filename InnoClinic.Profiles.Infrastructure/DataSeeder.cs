using InnoClinic.Profiles.Domain.Entities;
using InnoClinic.Profiles.Domain.Entities.Users;
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
            if (!await _context.Accounts.AnyAsync())
            {
                var accounts = new List<Account>()
                {
                    // Doctors
                    new()
                    {
                        Id = Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381"),
                        Email = "elena.volkova@example.com",
                        Password = "Elena@2024",
                        PhoneNumber = "+7-495-1234567",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397"),
                        Email = "sergey.ivanov@example.com",
                        Password = "Sergey@2023",
                        PhoneNumber = "+7-812-9876543",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddMonths(-12),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"),
                        Email = "amina.sadikova@example.com",
                        Password = "Amina@2024",
                        PhoneNumber = "+7-383-4567890",
                        IsEmailVerified = false,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddMonths(-6),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    // Receptionists
                    new Account
                    {
                        Id = Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5"),
                        Email = "olga.smirnova@clinic.com",
                        Password = "Olga#2024",
                        PhoneNumber = "+375-29-6543210",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6"),
                        Email = "mateusz.kowalski@clinic.com",
                        Password = "Mateusz#2024",
                        PhoneNumber = "+48-22-7891234",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddDays(-80),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7"),
                        Email = "leyla.abdulova@clinic.com",
                        Password = "Leyla#2024",
                        PhoneNumber = "+994-12-4567890",
                        IsEmailVerified = false,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddDays(-40),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    // Patients
                    new Account
                    {
                        Id = Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107"),
                        Email = "maxim.petrov@patientmail.com",
                        Password = "Maxim2024!",
                        PhoneNumber = "+7-911-1112222",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f"),
                        Email = "charlotte.bergman@patientmail.com",
                        Password = "Charlotte2024!",
                        PhoneNumber = "+49-30-2223334",
                        IsEmailVerified = false,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddDays(-120),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    },
                    new Account
                    {
                        Id = Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2"),
                        Email = "rajesh.mehta@patientmail.com",
                        Password = "Rajesh2024!",
                        PhoneNumber = "+91-22-4561230",
                        IsEmailVerified = true,
                        PhotoId = Guid.NewGuid(),
                        CreatedBy = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow.AddDays(-365),
                        UpdatedBy = Guid.NewGuid(),
                        UpdatedOn = DateTime.UtcNow
                    }
                };
                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();
            }

            if (!await _context.Doctors.AnyAsync())
            {
                var doctors = new List<Doctor>()
                {
                    new Doctor
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Elena",
                        LastName = "Volkova",
                        MiddleName = "Petrovna",
                        AccountId = Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381"),
                        DateOfBirth = new DateTime(1985, 4, 12),
                        SpecializationId = Guid.Parse("c7d96582-64fd-42fc-940a-a671ff10fa0e"),
                        OfficeId = Guid.Parse("9a1bfb14-1f48-43f3-8133-f6b1e9d57d6d"),
                        CareerStartYear = 2010,
                        Status = DoctorStatus.AtWork
                    },
                    new Doctor
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Sergey",
                        LastName = "Ivanov",
                        MiddleName = "Mikhailovich",
                        AccountId = Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397"),
                        DateOfBirth = new DateTime(1978, 9, 30),
                        SpecializationId = Guid.Parse("e1b7be59-12e3-4b20-bc66-df91c63ec772"),
                        OfficeId = Guid.Parse("2fa1cb6e-b74c-40e0-a7ca-c9f9e29462a0"),
                        CareerStartYear = 2003,
                        Status = DoctorStatus.Inactive
                    },
                    new Doctor
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Amina",
                        LastName = "Sadikova",
                        MiddleName = "Nurullaevna",
                        AccountId = Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"),
                        DateOfBirth = new DateTime(1990, 1, 22),
                        SpecializationId = Guid.Parse("6e5d46fc-c0c6-4e1f-a3e8-382a7be787ec"),
                        OfficeId = Guid.Parse("b0cb9253-4a21-4171-9b2a-5bfb1e27e992"),
                        CareerStartYear = 2015,
                        Status = DoctorStatus.OnVacation
                    }
                };

                await _context.Doctors.AddRangeAsync(doctors);
            }

            if (!await _context.Receptionists.AnyAsync())
            {
                var receptionists = new List<Receptionist>()
                {
                    new Receptionist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Olga",
                        LastName = "Smirnova",
                        MiddleName = "Nikolaevna",
                        AccountId = Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5"),
                        OfficeId = Guid.Parse("a4de348a-915b-4fe2-b7ad-30f9e446de51")
                    },
                    new Receptionist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Mateusz",
                        LastName = "Kowalski",
                        MiddleName = "Jerzy",
                        AccountId = Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6"),
                        OfficeId = Guid.Parse("7c2069fd-8bf8-4269-b34f-dbbf6fc54702")
                    },
                    new Receptionist
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Leyla",
                        LastName = "Abdulova",
                        MiddleName = "Rasimovna",
                        AccountId = Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7"),
                        OfficeId = Guid.Parse("9f32bd63-4e95-499f-9f20-2bb60de5c234")
                    }
                };
                
                await _context.Receptionists.AddRangeAsync(receptionists);
            }

            if (!await _context.Patients.AnyAsync())
            {
                var patients = new List<Patient>()
                {
                    new Patient
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Maxim",
                        LastName = "Petrov",
                        MiddleName = "Ivanovich",
                        AccountId = Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107"),
                        DateOfBirth = new DateTime(1995, 7, 18)
                    },
                    new Patient
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Charlotte",
                        LastName = "Bergman",
                        MiddleName = "Louise",
                        AccountId = Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f"),
                        DateOfBirth = new DateTime(2002, 11, 3)
                    },
                    new Patient
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Rajesh",
                        LastName = "Mehta",
                        MiddleName = "Anilkumar",
                        AccountId = Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2"),
                        DateOfBirth = new DateTime(1988, 3, 29)
                    }
                };
                
                await _context.Patients.AddRangeAsync(patients);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}