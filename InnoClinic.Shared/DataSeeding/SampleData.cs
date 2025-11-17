using InnoClinic.Shared.DataSeeding.Entities;
using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;
using InnoClinic.Shared.DataSeeding.Entities.Services;

namespace InnoClinic.Shared.DataSeeding
{
    /// <summary>
    /// This class contains sample data for seeding APIs databases with initial values.
    /// Password for all Receptionists and Doctors accounts: 123456,
    /// Password for all Patients accounts: Aa123456!
    /// </summary>
    public static class SampleData
    {
        public const string StrongPassword = "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==";
        public const string EasyPassword = "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==";
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
        public static readonly Guid IvanPetrov = Guid.Parse("9a8b7c6d-5e4f-4a3b-9c2d-1e0f9a8b7c6d");
        public static readonly Guid LucasMartinez = Guid.Parse("c1d2e3f4-5a6b-7c8d-9e0f-112233445566");
        public static readonly Guid SophiaChen = Guid.Parse("8f3c2b1a-4d5e-6789-abcd-1234567890ef");
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
        #region Specializations IDs
        public static readonly Guid Cardiology = Guid.Parse("c7d96582-64fd-42fc-940a-a671ff10fa0e");
        public static readonly Guid Dermatology = Guid.Parse("e1b7be59-12e3-4b20-bc66-df91c63ec772");
        public static readonly Guid Neurology = Guid.Parse("6e5d46fc-c0c6-4e1f-a3e8-382a7be787ec");
        public static readonly Guid Radiology = Guid.Parse("9a8b7c6d-5e4f-4a3b-9c2d-1e0f9a8b7c6d");
        public static readonly Guid Hematology = Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f");
        public static readonly Guid Urology = Guid.Parse("7a8b9c0d-1e2f-3a4b-5c6d-7e8f9a0b1c2d");
        public static readonly Guid Gastro = Guid.Parse("0c1f2a3b-4d5e-6789-abcd-1234567890aa");
        public static readonly Guid Endo = Guid.Parse("1d2e3f4a-5b6c-7d8e-9f01-2345678901bb");
        public static readonly Guid Infectious = Guid.Parse("2e3f4a5b-6c7d-8e9f-0123-4567890122cc");
        #endregion
        #region Service Categories IDs
        public static readonly Guid Consultations = Guid.Parse("3f4a5b6c-7d8e-9f01-2345-6789012333dd");
        public static readonly Guid Diagnostics = Guid.Parse("4a5b6c7d-8e9f-0123-4567-8901234444ee");
        public static readonly Guid Analyses = Guid.Parse("5b6c7d8e-9f01-2345-6789-0123455555ff");
        #endregion
        #region Photos IDs
        public static readonly Guid EVolkovaPhotoId = Guid.Parse("1f4a7c2b-8e3d-4b9a-9f1c-2a6d5e7b8c9d");
        public static readonly Guid SIvanovPhotoId = Guid.Parse("a2b3c4d5-e6f7-48a9-b0c1-d2e3f4a5b6c7");
        public static readonly Guid ASadikovaPhotoId = Guid.Parse("9d8c7b6a-5e4f-4d3c-8b2a-1f0e9d8c7b6a");
        public static readonly Guid IPetrovPhotoId = Guid.Parse("f6a4b3c2-8d9e-4b1a-9c3d-2e7f8a6b5c4d");
        public static readonly Guid LMartinezPhotoId = Guid.Parse("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d");
        public static readonly Guid SChenPhotoId = Guid.Parse("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b");
        public static readonly Guid OSmirnovaPhotoId = Guid.Parse("c1d2e3f4-a5b6-47c8-9d0e-1f2a3b4c5d6e");
        public static readonly Guid MKowalskiPhotoId = Guid.Parse("e7f8a9b0-c1d2-4e3f-8a9b-0c1d2e3f4a5b");
        public static readonly Guid LAbdulovaPhotoId = Guid.Parse("b6c7d8e9-f0a1-42b3-9c4d-5e6f7a8b9c0d");
        public static readonly Guid MPetrovPhotoId = Guid.Parse("0a1b2c3d-4e5f-46a7-8b9c-0d1e2f3a4b5c");
        public static readonly Guid CBergmanPhotoId = Guid.Parse("54f8c2a1-3b7d-4f6e-9a8c-2d1b4e5f7c8a");
        public static readonly Guid RMehtaPhotoId = Guid.Parse("3c4d5e6f-7a8b-4c9d-8e0f-1a2b3c4d5e6f");
        public static readonly Guid MinskPhotoId = Guid.Parse("b7c8d9e0-1a2b-4c3d-8e9f-0a1b2c3d4e5f");
        public static readonly Guid GrodnoPhotoId = Guid.Parse("d2e3f4a5-b6c7-4d8e-9f0a-1b2c3d4e5f6a");
        public static readonly Guid BrestPhotoId = Guid.Parse("f4a5b6c7-d8e9-4f0a-1b2c-3d4e5f6a7b8c");
        public static readonly Guid VitebskPhotoId = Guid.Parse("b6c7d8e9-f0a1-4b2c-3d4e-5f6a7b8c9d0e");
        public static readonly Guid MogilevPhotoId = Guid.Parse("d8e9f0a1-b2c3-4d5e-6f7a-8b9c0d1e2f3a");
        public static readonly Guid GomelPhotoId = Guid.Parse("f0a1b2c3-d4e5-4f6a-7b8c-9d0e1f2a3b4c");
        #endregion

        public static Dictionary<Guid, Account<Doctor>> Doctors { get; } = new()
        {
            { ElenaVolkova,
                new Account<Doctor>
                {
                    Email = "elena.volkova@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber =  "+7-495-1234567",
                    PhotoId = EVolkovaPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("0f1a2b3c-4d5e-4f6a-8b7c-9d0e1f2a3b4c"),
                        FirstName = "Elena",
                        LastName = "Volkova",
                        MiddleName = "Petrovna",
                        DateOfBirth = new DateTime(1985, 4, 12),
                        SpecializationId = Cardiology,
                        OfficeId = Brest,
                        CareerStartYear = 2010,
                        Status = DoctorStatus.AtWork
                    }
                }
            },
            { SergeyIvanov,
                new Account<Doctor>
                {
                    Email = "sergey.ivanov@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+7-812-9876543",
                    PhotoId = SIvanovPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("d4e5f6a7-b8c9-4d0e-8f1a-2b3c4d5e6f7a"),
                        FirstName = "Sergey",
                        LastName = "Ivanov",
                        MiddleName = "Mikhailovich",
                        DateOfBirth = new DateTime(1978, 9, 30),
                        SpecializationId = Dermatology,
                        OfficeId = Grodno,
                        CareerStartYear = 2003,
                        Status = DoctorStatus.Inactive
                    }
                }
            },
            { AminaSadikova,
                new Account<Doctor>
                {
                    Email = "amina.sadikova@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+7-383-4567890",
                    PhotoId = ASadikovaPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("6a7b8c9d-0e1f-4a2b-9c3d-4e5f6a7b8c9d"),
                        FirstName = "Amina",
                        LastName = "Sadikova",
                        MiddleName = "Nurullaevna",
                        DateOfBirth = new DateTime(1990, 1, 22),
                        SpecializationId = Neurology,
                        OfficeId = Minsk,
                        CareerStartYear = 2015,
                        Status = DoctorStatus.OnVacation
                    }
                }
            },
            { IvanPetrov,
                new Account<Doctor>
                {
                    Email = "ivan.petrov@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+7-495-1234567",
                    PhotoId = IPetrovPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("3c2d1b4a-7e8f-4a9b-b2c3-d4e5f6a7b8c9"),
                        FirstName = "Ivan",
                        LastName = "Petrov",
                        MiddleName = "Sergeevich",
                        DateOfBirth = new DateTime(1985, 5, 14),
                        SpecializationId = Gastro,
                        OfficeId = Grodno,
                        CareerStartYear = 2010,
                        Status = DoctorStatus.AtWork
                    }
                }
            },
            { LucasMartinez,
                new Account<Doctor>
                {
                    Email = "lucas.martinez@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+34-91-6543210",
                    PhotoId = LMartinezPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"),
                        FirstName = "Lucas",
                        LastName = "Martinez",
                        MiddleName = "Alejandro",
                        DateOfBirth = new DateTime(1982, 9, 3),
                        SpecializationId = Hematology,
                        OfficeId = Minsk,
                        CareerStartYear = 2008,
                        Status = DoctorStatus.AtWork
                    }
                }
            },
            { SophiaChen,
                new Account<Doctor>
                {
                    Email = "sophia.chen@example.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+1-212-9876543",
                    PhotoId = SChenPhotoId,
                    Profile = new Doctor
                    {
                        Id = Guid.Parse("6f7a8b9c-0d1e-2f3a-4b5c-6d7e8f9a0b1c"),
                        FirstName = "Sophia",
                        LastName = "Chen",
                        MiddleName = "MeiLing",
                        DateOfBirth = new DateTime(1992, 12, 7),
                        SpecializationId = Endo,
                        OfficeId = Mogilev,
                        CareerStartYear = 2017,
                        Status = DoctorStatus.AtWork
                    }
                }
            }
        };
        public static Dictionary<Guid, Account<Receptionist>> Receptionists { get; } = new()
        {
            { OlgaSmirnova,
                new Account<Receptionist>
                {
                    Email = "olga.smirnova@clinic.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+375-29-6543210",
                    PhotoId = OSmirnovaPhotoId,
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("9f0a1b2c-3d4e-4f5a-8b6c-7d8e9f0a1b2c"),
                        FirstName = "Olga",
                        LastName = "Smirnova",
                        MiddleName = "Nikolaevna",
                        OfficeId = Gomel
                    }
                }
            },
            { MateuszKowalski,
                new Account<Receptionist>
                {
                    Email = "mateusz.kowalski@clinic.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+48-22-7891234",
                    PhotoId = MKowalskiPhotoId,
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("5e6f7a8b-9c0d-4e1f-8a2b-3c4d5e6f7a8b"),
                        FirstName = "Mateusz",
                        LastName = "Kowalski",
                        MiddleName = "Jerzy",
                        OfficeId = Brest
                    }
                }
            },
            { LeylaAbdulova,
                new Account<Receptionist>
                {
                    Email = "leyla.abdulova@clinic.com",
                    PasswordHash = EasyPassword,
                    PhoneNumber = "+994-12-4567890",
                    PhotoId = LAbdulovaPhotoId,
                    Profile = new Receptionist
                    {
                        Id = Guid.Parse("2b3c4d5e-6f7a-4b8c-9d0e-1f2a3b4c5d6e"),
                        FirstName = "Leyla",
                        LastName = "Abdulova",
                        MiddleName = "Rasimovna",
                        OfficeId = Minsk
                    }
                }
            }
        };
        public static Dictionary<Guid, Account<Patient>> Patients { get; } = new()
        {
            { MaximPetrov,
                new Account<Patient>
                {
                    Email = "maxim.petrov@patientmail.com",
                    PasswordHash = StrongPassword,
                    PhoneNumber = "+7-911-1112222",
                    PhotoId = MPetrovPhotoId,
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
            { CharlotteBergman,
                new Account<Patient>
                {
                    Email = "charlotte.bergman@patientmail.com",
                    PasswordHash = StrongPassword,
                    PhoneNumber = "+49-30-2223334",
                    PhotoId = CBergmanPhotoId,
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
            { RajeshMehta,
                new Account<Patient>
                {
                    Email = "rajesh.mehta@patientmail.com",
                    PasswordHash = StrongPassword,
                    PhoneNumber = "+91-22-4561230",
                    PhotoId = RMehtaPhotoId,
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
            { Minsk,
                new Office
                {
                    Address = new Address
                    {
                        City = "Minsk",
                        Street = "Independence Avenue",
                        HouseNumber = "25",
                        OfficeNumber = "12"
                    },
                    PhotoId = MinskPhotoId,
                    RegistryPhoneNumber = "+375171234567",
                    IsActive = true
                }
            },
            { Grodno,
                new Office
                {
                    Address = new Address
                    {
                        City = "Grodno",
                        Street = "Sovetskaya Street",
                        HouseNumber = "48",
                        OfficeNumber = "5"
                    },
                    PhotoId = GrodnoPhotoId,
                    RegistryPhoneNumber = "+375152123456",
                    IsActive = true
                }
            },
            { Brest,
                new Office
                {
                    Address = new Address
                    {
                        City = "Brest",
                        Street = "Lenin Street",
                        HouseNumber = "10",
                        OfficeNumber = "101"
                    },
                    PhotoId = BrestPhotoId,
                    RegistryPhoneNumber = "+375162234567",
                    IsActive = true
                }
            },
            { Vitebsk,
                new Office
                {
                    Address = new Address
                    {
                        City = "Vitebsk",
                        Street = "Frunze Avenue",
                        HouseNumber = "72",
                        OfficeNumber = "8"
                    },
                    PhotoId = VitebskPhotoId,
                    RegistryPhoneNumber = "+375212345678",
                    IsActive = false
                }
            },
            { Mogilev,
                new Office
                {
                    Address = new Address
                    {
                        City = "Mogilev",
                        Street = "Pervomayskaya Street",
                        HouseNumber = "15",
                        OfficeNumber = "23"
                    },
                    PhotoId = MogilevPhotoId,
                    RegistryPhoneNumber = "+375222456789",
                    IsActive = false
                }
            },
            { Gomel,
                new Office
                {
                    Address = new Address
                    {
                        City = "Gomel",
                        Street = "Lenin Avenue",
                        HouseNumber = "50",
                        OfficeNumber = "7"
                    },
                    PhotoId = GomelPhotoId,
                    RegistryPhoneNumber = "+375232456789",
                    IsActive = true
                }
            }
        };
        public static Dictionary<Guid, Specialization> Specializations { get; } = new()
        {
            { Cardiology,
                new Specialization
                {
                    Name = "Cardiology",
                    IsActive = true
                }
            },
            { Dermatology,
                new Specialization
                {
                    Name = "Dermatology",
                    IsActive = true
                }
            },
            { Neurology,
                new Specialization
                {
                    Name = "Neurology",
                    IsActive = true
                }
            },
            { Gastro,
                new Specialization
                {
                    Name = "Gastroenterology",
                    IsActive = true
                }
            },
            { Endo,
                new Specialization
                {
                    Name = "Endocrinology",
                    IsActive = true
                }
            },
            { Radiology,
                new Specialization
                {
                    Name = "Radiology",
                    IsActive = false
                }
            },
            { Hematology,
                new Specialization
                {
                    Name = "Hematology",
                    IsActive = true
                }
            },
            { Infectious,
                new Specialization
                {
                    Name = "Infectious Disease",
                    IsActive = false
                }
            },
            { Urology,
                new Specialization
                {
                    Name = "Urology",
                    IsActive = false
                }
            }
        };
        public static Dictionary<Guid, ServiceCategory> ServiceCategories { get; } = new()
        {
            { Consultations, 
                new ServiceCategory
                {
                    Name = "Consultations",
                    TimeSlotSize = TimeSpan.FromMinutes(30)
                }
            },
            { Diagnostics, 
                new ServiceCategory
                {
                    Name = "Diagnostics",
                    TimeSlotSize = TimeSpan.FromMinutes(60)
                }
            },
            { Analyses, 
                new ServiceCategory
                {
                    Name = "Analyses",
                    TimeSlotSize = TimeSpan.FromMinutes(15)
                }
            }
        };
        public static Dictionary<Guid, Service> Services { get; } = new()
        {
            { Guid.Parse("6c7d8e9f-0123-4567-8901-2345666666aa"),
                new Service
                { 
                    Name = "Cardiologist Consultation",
                    CategoryId = Consultations,
                    SpecializationId = Cardiology,
                    Price = 60.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("7d8e9f01-2345-6789-0123-4567777777bb"),
                new Service
                {
                    Name = "Dermatologist Consultation",
                    CategoryId = Consultations,
                    SpecializationId = Dermatology,
                    Price = 55.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("8e9f0123-4567-8901-2345-6789888888cc"),
                new Service
                { 
                    Name = "Neurologist Consultation",
                    CategoryId = Consultations,
                    SpecializationId = Neurology,
                    Price = 65.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("9f012345-6789-0123-4567-8999999999dd"),
                new Service
                { 
                    Name = "Gastroenterologist Consultation",
                    CategoryId = Consultations,
                    SpecializationId = Gastro,
                    Price = 58.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("a0123456-7890-1234-5678-9000000000ee"),
                new Service
                {
                    Name = "Endocrinologist Consultation",
                    CategoryId = Consultations,
                    SpecializationId = Endo,
                    Price = 62.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("b1234567-8901-2345-6789-0111111111ff"),
                new Service
                {
                    Name = "Electrocardiogram (ECG)",
                    CategoryId = Diagnostics,
                    SpecializationId = Cardiology,
                    Price = 40.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("c2345678-9012-3456-7890-1222222222aa"),
                new Service
                {
                    Name = "Skin Biopsy",
                    CategoryId = Diagnostics,
                    SpecializationId = Dermatology,
                    Price = 70.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("d3456789-0123-4567-8901-2333333333bb"),
                new Service
                {
                    Name = "Electroencephalogram (EEG)",
                    CategoryId = Diagnostics,
                    SpecializationId = Neurology,
                    Price = 75.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("e4567890-1234-5678-9012-3444444444cc"), 
                new Service
                {
                    Name = "Abdominal Ultrasound",
                    CategoryId = Diagnostics,
                    SpecializationId = Gastro,
                    Price = 80.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("f5678901-2345-6789-0123-4555555555dd"),
                new Service
                {
                    Name = "Thyroid Ultrasound",
                    CategoryId = Diagnostics,
                    SpecializationId = Endo,
                    Price = 65.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("01234567-89ab-cdef-0123-4566666666ee"),
                new Service
                {
                    Name = "Complete Blood Count",
                    CategoryId = Analyses,
                    SpecializationId = Hematology,
                    Price = 25.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("12345678-9abc-def0-1234-5677777777ff"),
                new Service
                {
                    Name = "COVID-19 PCR Test",
                    CategoryId = Analyses,
                    SpecializationId = Infectious,
                    Price = 30.00f,
                    IsActive = false
                }
            },
            { Guid.Parse("23456789-abcd-ef01-2345-6788888888aa"),
                new Service
                {
                    Name = "Urinalysis",
                    CategoryId = Analyses,
                    SpecializationId = Urology,
                    Price = 20.00f,
                    IsActive = false
                }
            },
            { Guid.Parse("3456789a-bcde-f012-3456-7899999999bb"),
                new Service
                {
                    Name = "Blood Glucose Test",
                    CategoryId = Analyses,
                    SpecializationId = Endo,
                    Price = 22.00f,
                    IsActive = true
                }
            },
            { Guid.Parse("456789ab-cdef-0123-4567-8900000000cc"),
                new Service
                {
                    Name = "Liver Function Test",
                    CategoryId = Analyses,
                    SpecializationId = Gastro,
                    Price = 28.00f,
                    IsActive = true
                }
            }
        };
        public static Dictionary<Guid, string> Photos { get; } = new()
        {
            {
                EVolkovaPhotoId,
                "Photos/Doctors/E_Volkova.png"
            },
            {
                SIvanovPhotoId,
                "Photos/Doctors/S_Ivanov.png"
            },
            {
                ASadikovaPhotoId,
                "Photos/Doctors/A_Sadikova.png"
            },
            {
                IPetrovPhotoId,
                "Photos/Doctors/I_Petrov.png"
            },
            {
                LMartinezPhotoId,
                "Photos/Doctors/L_Martinez.png"
            },
            {
                SChenPhotoId,
                "Photos/Doctors/S_Chen.png"
            },
            {
                OSmirnovaPhotoId,
                "Photos/Receptionists/O_Smirnova.png"
            },
            {
                MKowalskiPhotoId,
                "Photos/Receptionists/M_Kowalski.png"
            },
            {
                LAbdulovaPhotoId,
                "Photos/Receptionists/L_Abdulova.png"
            },
            {
                MPetrovPhotoId,
                "Photos/Patients/M_Petrov.png"
            },
            {
                CBergmanPhotoId,
                "Photos/Patients/C_Bergman.png"
            },
            {
                RMehtaPhotoId,
                "Photos/Patients/R_Mehta.png"
            },
            {
                MinskPhotoId,
                "Photos/Offices/Minsk.png"
            },
            {
                GrodnoPhotoId,
                "Photos/Offices/Grodno.png"
            },
            {
                BrestPhotoId,
                "Photos/Offices/Brest.png"
            },
            {
                VitebskPhotoId,
                "Photos/Offices/Vitebsk.png"
            },
            {
                MogilevPhotoId,
                "Photos/Offices/Mogilev.png"
            },
            {
                GomelPhotoId,
                "Photos/Offices/Gomel.png"
            }
        };
    }
}