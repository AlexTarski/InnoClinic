using InnoClinic.Shared.DataSeeding.Entities;
using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;

namespace InnoClinic.Shared.DataSeeding
{
    /// <summary>
    /// This class contains sample data for seeding APIs databases with initial values.
    /// Password for all Receptionists and Doctors accounts: 123456,
    /// Password for all Patients accounts: Aa123456!
    /// </summary>
    public static class SampleData
    {
        #region Offices IDs
        public static readonly Guid Minsk = Guid.Parse("a3f2b6c1-4d8e-4a2b-9f6a-1c2d3e4f5a6b");
        public static readonly Guid Grodno = Guid.Parse("c1d2e3f4-5a6b-4c7d-8e9f-0a1b2c3d4e5f");
        public static readonly Guid Brest = Guid.Parse("e3f4a5b6-c7d8-4e9f-0a1b-2c3d4e5f6a7b");
        public static readonly Guid Vitebsk = Guid.Parse("a5b6c7d8-e9f0-4a1b-2c3d-4e5f6a7b8c9d");
        public static readonly Guid Mogilev = Guid.Parse("c7d8e9f0-a1b2-4c3d-5e6f-7a8b9c0d1e2f");
        public static readonly Guid Gomel = Guid.Parse("e9f0a1b2-c3d4-4e5f-6a7b-8c9d0e1f2a3b");
        #endregion
        #region Doctors IDs
        public static readonly Guid ElenaVolkova = Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381");
        public static readonly Guid SergeyIvanov = Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397");
        public static readonly Guid AminaSadikova = Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0");
        #endregion
        #region Receptionists IDs
        public static readonly Guid OlgaSmirnova = Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5");
        public static readonly Guid MateuszKowalski = Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6");
        public static readonly Guid LeylaAbdulova = Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7");
        #endregion
        #region Patients IDs
        public static readonly Guid MaximPetrov = Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107");
        public static readonly Guid CharlotteBergman = Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f");
        public static readonly Guid RajeshMehta = Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2");
        #endregion

        public static Dictionary<Guid, Account<Doctor>> Doctors { get; } = new()
        {
            { Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381"),
                new Account<Doctor>
                {
                    Email = "elena.volkova@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber =  "+7-495-1234567",
                    PhotoId = Guid.Parse("1f4a7c2b-8e3d-4b9a-9f1c-2a6d5e7b8c9d"),
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("0f1a2b3c-4d5e-4f6a-8b7c-9d0e1f2a3b4c"),
                        FirstName = "Elena",
                        LastName = "Volkova",
                        MiddleName = "Petrovna",
                        DateOfBirth = new DateTime(1985, 4, 12),
                        SpecializationId = Guid.Parse("c7d96582-64fd-42fc-940a-a671ff10fa0e"),
                        OfficeId = Guid.Parse("e3f4a5b6-c7d8-4e9f-0a1b-2c3d4e5f6a7b"),
                        CareerStartYear = 2010,
                        Status = DoctorStatus.AtWork
                    }
                }
            },
            { Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397"),
                new Account<Doctor>
                {
                    Email = "sergey.ivanov@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber = "+7-812-9876543",
                    PhotoId = Guid.Parse("a2b3c4d5-e6f7-48a9-b0c1-d2e3f4a5b6c7"),
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("d4e5f6a7-b8c9-4d0e-8f1a-2b3c4d5e6f7a"),
                        FirstName = "Sergey",
                        LastName = "Ivanov",
                        MiddleName = "Mikhailovich",
                        DateOfBirth = new DateTime(1978, 9, 30),
                        SpecializationId = Guid.Parse("e1b7be59-12e3-4b20-bc66-df91c63ec772"),
                        OfficeId = Guid.Parse("c1d2e3f4-5a6b-4c7d-8e9f-0a1b2c3d4e5f"),
                        CareerStartYear = 2003,
                        Status = DoctorStatus.Inactive
                    }
                }
            },
            { Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"),
                new Account<Doctor>
                {
                    Email = "amina.sadikova@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber = "+7-383-4567890",
                    PhotoId = Guid.Parse("9d8c7b6a-5e4f-4d3c-8b2a-1f0e9d8c7b6a"),
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("6a7b8c9d-0e1f-4a2b-9c3d-4e5f6a7b8c9d"),
                        FirstName = "Amina",
                        LastName = "Sadikova",
                        MiddleName = "Nurullaevna",
                        DateOfBirth = new DateTime(1990, 1, 22),
                        SpecializationId = Guid.Parse("6e5d46fc-c0c6-4e1f-a3e8-382a7be787ec"),
                        OfficeId = Guid.Parse("a3f2b6c1-4d8e-4a2b-9f6a-1c2d3e4f5a6b"),
                        CareerStartYear = 2015,
                        Status = DoctorStatus.OnVacation
                    }
                }
            }
        };
        public static Dictionary<Guid, Account<Receptionist>> Receptionists { get; } = new()
        {
            { Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5"),
                new Account<Receptionist>
                {
                    Email = "olga.smirnova@clinic.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber = "+375-29-6543210",
                    PhotoId = Guid.Parse("c1d2e3f4-a5b6-47c8-9d0e-1f2a3b4c5d6e"),
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("9f0a1b2c-3d4e-4f5a-8b6c-7d8e9f0a1b2c"),
                        FirstName = "Olga",
                        LastName = "Smirnova",
                        MiddleName = "Nikolaevna",
                        OfficeId = Guid.Parse("e9f0a1b2-c3d4-4e5f-6a7b-8c9d0e1f2a3b")
                    }
                }
            },
            { Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6"),
                new Account<Receptionist>
                {
                    Email = "mateusz.kowalski@clinic.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber = "+48-22-7891234",
                    PhotoId = Guid.Parse("e7f8a9b0-c1d2-4e3f-8a9b-0c1d2e3f4a5b"),
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("5e6f7a8b-9c0d-4e1f-8a2b-3c4d5e6f7a8b"),
                        FirstName = "Mateusz",
                        LastName = "Kowalski",
                        MiddleName = "Jerzy",
                        OfficeId = Guid.Parse("e3f4a5b6-c7d8-4e9f-0a1b-2c3d4e5f6a7b")
                    }
                }
            },
            { Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7"),
                new Account<Receptionist>
                {
                    Email = "leyla.abdulova@clinic.com",
                    PasswordHash = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                    PhoneNumber = "+994-12-4567890",
                    PhotoId = Guid.Parse("b6c7d8e9-f0a1-42b3-9c4d-5e6f7a8b9c0d"),
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("2b3c4d5e-6f7a-4b8c-9d0e-1f2a3b4c5d6e"),
                        FirstName = "Leyla",
                        LastName = "Abdulova",
                        MiddleName = "Rasimovna",
                        OfficeId = Guid.Parse("a3f2b6c1-4d8e-4a2b-9f6a-1c2d3e4f5a6b")
                    }
                }
            }
        };
        public static Dictionary<Guid, Account<Patient>> Patients { get; } = new()
        {
            { Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107"),
                new Account<Patient>
                {
                    Email = "maxim.petrov@patientmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                    PhoneNumber = "+7-911-1112222",
                    PhotoId = Guid.Parse("0a1b2c3d-4e5f-46a7-8b9c-0d1e2f3a4b5c"),
                    Profile = new Patient
                    {
                        Id = Guid.Parse("7a8b9c0d-1e2f-4a3b-9c4d-5e6f7a8b9c0d"),
                        FirstName = "Maxim",
                        LastName = "Petrov",
                        MiddleName = "Ivanovich",
                        DateOfBirth = new DateTime(1995, 7, 18)
                    }
                }
            },
            { Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f"),
                new Account<Patient>
                {
                    Email = "charlotte.bergman@patientmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                    PhoneNumber = "+49-30-2223334",
                    PhotoId = Guid.Parse("54f8c2a1-3b7d-4f6e-9a8c-2d1b4e5f7c8a"),
                    Profile = new Patient
                    {
                        Id = Guid.Parse("4e5f6a7b-8c9d-4e0f-8a1b-2c3d4e5f6a7b"),
                        FirstName = "Charlotte",
                        LastName = "Bergman",
                        MiddleName = "Louise",
                        DateOfBirth = new DateTime(2002, 11, 3)
                    }
                }
            },
            { Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2"),
                new Account<Patient>
                {
                    Email = "rajesh.mehta@patientmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                    PhoneNumber = "+91-22-4561230",
                    PhotoId = Guid.Parse("3c4d5e6f-7a8b-4c9d-8e0f-1a2b3c4d5e6f"),
                    Profile = new Patient
                    {
                        Id = Guid.Parse("8b9c0d1e-2f3a-4b5c-8d9e-0f1a2b3c4d5e"),
                        FirstName = "Rajesh",
                        LastName = "Mehta",
                        MiddleName = "Anilkumar",
                        DateOfBirth = new DateTime(1988, 3, 29)
                    }
                }
            }
        };
        public static Dictionary<Guid, Office> Offices { get; } = new()
        {
            { Guid.Parse("a3f2b6c1-4d8e-4a2b-9f6a-1c2d3e4f5a6b"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Minsk",
                        Street = "Independence Avenue",
                        HouseNumber = "25",
                        OfficeNumber = "12"
                    },
                    PhotoId = Guid.Parse("b7c8d9e0-1a2b-4c3d-8e9f-0a1b2c3d4e5f"),
                    RegistryPhoneNumber = "+375171234567",
                    IsActive = true
                }
            },
            { Guid.Parse("c1d2e3f4-5a6b-4c7d-8e9f-0a1b2c3d4e5f"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Grodno",
                        Street = "Sovetskaya Street",
                        HouseNumber = "48",
                        OfficeNumber = "5"
                    },
                    PhotoId = Guid.Parse("d2e3f4a5-b6c7-4d8e-9f0a-1b2c3d4e5f6a"),
                    RegistryPhoneNumber = "+375152123456",
                    IsActive = true
                }
            },
            { Guid.Parse("e3f4a5b6-c7d8-4e9f-0a1b-2c3d4e5f6a7b"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Brest",
                        Street = "Lenin Street",
                        HouseNumber = "10",
                        OfficeNumber = "101"
                    },
                    PhotoId = Guid.Parse("f4a5b6c7-d8e9-4f0a-1b2c-3d4e5f6a7b8c"),
                    RegistryPhoneNumber = "+375162234567",
                    IsActive = true
                }
            },
            { Guid.Parse("a5b6c7d8-e9f0-4a1b-2c3d-4e5f6a7b8c9d"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Vitebsk",
                        Street = "Frunze Avenue",
                        HouseNumber = "72",
                        OfficeNumber = "8"
                    },
                    PhotoId = Guid.Parse("b6c7d8e9-f0a1-4b2c-3d4e-5f6a7b8c9d0e"),
                    RegistryPhoneNumber = "+375212345678",
                    IsActive = false
                }
            },
            { Guid.Parse("c7d8e9f0-a1b2-4c3d-5e6f-7a8b9c0d1e2f"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Mogilev",
                        Street = "Pervomayskaya Street",
                        HouseNumber = "15",
                        OfficeNumber = "23"
                    },
                    PhotoId = Guid.Parse("d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a"),
                    RegistryPhoneNumber = "+375222456789",
                    IsActive = false
                }
            },
            { Guid.Parse("e9f0a1b2-c3d4-4e5f-6a7b-8c9d0e1f2a3b"),
                new Office
                {
                    Address = new Address
                    {
                        City = "Gomel",
                        Street = "Lenin Avenue",
                        HouseNumber = "50",
                        OfficeNumber = "7"
                    },
                    PhotoId = Guid.Parse("f0a1b2c3-d4e5-4f6a-7b8c-9d0e1f2a3b4c"),
                    RegistryPhoneNumber = "+375232456789",
                    IsActive = true
                }
            }
        };
    }
}