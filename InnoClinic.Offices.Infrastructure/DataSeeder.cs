using InnoClinic.Offices.Domain;
using InnoClinic.Shared.DataSeeding;

using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Offices.Infrastructure
{
    public class DataSeeder
    {
        private readonly OfficesDbContext _context;
        private readonly Guid Minsk = Guid.Parse("a3f2b6c1-4d8e-4a2b-9f6a-1c2d3e4f5a6b");
        private readonly Guid Grodno = Guid.Parse("c1d2e3f4-5a6b-4c7d-8e9f-0a1b2c3d4e5f");
        private readonly Guid Brest = Guid.Parse("e3f4a5b6-c7d8-4e9f-0a1b-2c3d4e5f6a7b");
        private readonly Guid Vitebsk = Guid.Parse("a5b6c7d8-e9f0-4a1b-2c3d-4e5f6a7b8c9d");
        private readonly Guid Mogilev = Guid.Parse("c7d8e9f0-a1b2-4c3d-5e6f-7a8b9c0d1e2f");
        private readonly Guid Gomel = Guid.Parse("e9f0a1b2-c3d4-4e5f-6a7b-8c9d0e1f2a3b");

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
                    CreateOffice(Minsk),
                    CreateOffice(Grodno),
                    CreateOffice(Brest),
                    CreateOffice(Vitebsk),
                    CreateOffice(Mogilev),
                    CreateOffice(Gomel)
                    };

                await _context.Offices.AddRangeAsync(sampleOffices);
                await _context.SaveChangesAsync();
            }
        }
    }
}