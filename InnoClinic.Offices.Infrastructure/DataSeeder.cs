using InnoClinic.Offices.Domain;

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

        public async Task SeedAsync()
        {
            if (!await _context.Offices.AnyAsync())
            {
                var sampleOffices = new[]
                {
                    CreateOffice(Guid.NewGuid(), "City A", "Street A", "1A", "111", "+1-111-1111111", true),
                    CreateOffice(Guid.NewGuid(), "City B", "Street B", "1B", "222", "+0-000-0000000", false)
                    };

                await _context.Offices.AddRangeAsync(sampleOffices);
                await _context.SaveChangesAsync();
            }
        }

        private static Address CreateAddress(string city, string street, string houseNumber, string officeNumber) =>
            new()
            {
                City = city,
                Street = street,
                HouseNumber = houseNumber,
                OfficeNumber = officeNumber
            };

        private static Office CreateOffice(Guid id, string city, string street, string houseNumber, string officeNumber, string phoneNumber, bool isActive) =>
            new()
            {
                Id = id,
                Address = CreateAddress(city, street, houseNumber, officeNumber),
                RegistryPhoneNumber = phoneNumber,
                IsActive = isActive
            };
    }
}