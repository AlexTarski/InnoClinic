using InnoClinic.Offices.Domain;
using InnoClinic.Shared.DataSeeding;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Offices.Infrastructure
{
    public class DataSeeder
    {
        private readonly OfficesDbContext _context;

        public DataSeeder(OfficesDbContext context)
        {
            _context = context;
        }

        private static Address CreateAddress(string city, string street, string houseNumber, string officeNumber) =>
            new()
            {
                City = city,
                Street = street,
                HouseNumber = houseNumber,
                OfficeNumber = officeNumber
            };

        private static Office CreateOffice(Guid id)
        {
            var office = SampleData.Offices[id];
            return new Office()
            {
                Id = id,
                Address = CreateAddress(office.Address.City, office.Address.Street, office.Address.HouseNumber, office.Address.OfficeNumber),
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                PhotoId = office.PhotoId,
                IsActive = office.IsActive
            };
        }

        public async Task SeedAsync()
        {
            if (!await _context.Offices.AnyAsync())
            {
                var sampleOffices = new[]
                {
                    CreateOffice(SampleData.Minsk),
                    CreateOffice(SampleData.Grodno),
                    CreateOffice(SampleData.Brest),
                    CreateOffice(SampleData.Vitebsk),
                    CreateOffice(SampleData.Mogilev),
                    CreateOffice(SampleData.Gomel)
                    };

                await _context.Offices.AddRangeAsync(sampleOffices);
                await _context.SaveChangesAsync();
            }
        }
    }
}