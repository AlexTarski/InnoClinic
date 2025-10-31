using System;
using System.Threading.Tasks;

using InnoClinic.Documents.Domain.Entities;
using InnoClinic.Shared.DataSeeding;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Documents.Infrastructure
{
    public class DataSeeder
    {
        private readonly DocumentsContext _context;

        public DataSeeder(DocumentsContext context)
        { 
            _context = context;
        }

        private static Photo CreateDoctorPhoto(Guid accountId)
        {
            var doctorData = SampleData.Doctors[accountId];
            return CreatePhoto(doctorData.PhotoId);
        }

        private static Photo CreatePatientPhoto(Guid accountId)
        {
            var patientData = SampleData.Patients[accountId];
            return CreatePhoto(patientData.PhotoId);
        }

        private static Photo CreateReceptionistPhoto(Guid accountId)
        {
            var receptionistData = SampleData.Receptionists[accountId];
            return CreatePhoto(receptionistData.PhotoId);
        }

        private static Photo CreateOfficePhoto(Guid officeId)
        {
            var officeData = SampleData.Offices[officeId];
            return CreatePhoto(officeData.PhotoId);
        }

        private static Photo CreatePhoto(Guid photoId)
        {
            Photo photo = new ()
            {
                Id = photoId,
                Url = SampleData.Photos[photoId]
            };

            return photo;
        }

        public async Task SeedAsync()
        {
            if (!await _context.Photos.AnyAsync())
            {
                var photos = new Photo[]
                {
                    CreateDoctorPhoto(SampleData.ElenaVolkova),
                    CreateDoctorPhoto(SampleData.SergeyIvanov),
                    CreateDoctorPhoto(SampleData.AminaSadikova),
                    CreateDoctorPhoto(SampleData.IvanPetrov),
                    CreateDoctorPhoto(SampleData.LucasMartinez),
                    CreateDoctorPhoto(SampleData.SophiaChen),
                    CreateReceptionistPhoto(SampleData.OlgaSmirnova),
                    CreateReceptionistPhoto(SampleData.MateuszKowalski),
                    CreateReceptionistPhoto(SampleData.LeylaAbdulova),
                    CreatePatientPhoto(SampleData.MaximPetrov),
                    CreatePatientPhoto(SampleData.CharlotteBergman),
                    CreatePatientPhoto(SampleData.RajeshMehta),
                    CreateOfficePhoto(SampleData.Brest),
                    CreateOfficePhoto(SampleData.Gomel),
                    CreateOfficePhoto(SampleData.Grodno),
                    CreateOfficePhoto(SampleData.Vitebsk),
                    CreateOfficePhoto(SampleData.Mogilev),
                    CreateOfficePhoto(SampleData.Minsk),
                };

                await _context.AddRangeAsync(photos);
                await _context.SaveChangesAsync();
            }
        }
    }
}