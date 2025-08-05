using InnoClinic.Authorization.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.Infrastructure;

public class DataSeeder
{
    private readonly AuthorizationContext _context;

    public DataSeeder(AuthorizationContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {       
        if (!await _context.YourEntities.AnyAsync())
        {
            var newEntities = new List<YourEntity>()
            {
                new YourEntity
                {
                    Id = Guid.NewGuid(),
                    FullName = "Elena",
                    Ref2Id = Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381"),
                    Birth = new DateTime(1985, 4, 12),
                    RefId = Guid.Parse("9a1bfb14-1f48-43f3-8133-f6b1e9d57d6d"),
                    Year = 2010,
                    EntityStatus = YourEntityStatus.Status1
                },
                new YourEntity
                {
                    Id = Guid.NewGuid(),
                    FullName = "Sergey",
                    Ref2Id = Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397"),
                    Birth = new DateTime(1978, 9, 30),
                    RefId = Guid.Parse("2fa1cb6e-b74c-40e0-a7ca-c9f9e29462a0"),
                    Year = 2003,
                    EntityStatus = YourEntityStatus.Status2
                },
                new YourEntity
                {
                    Id = Guid.NewGuid(),
                    FullName = "Amina",
                    Ref2Id = Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"),
                    Birth = new DateTime(1990, 1, 22),
                    RefId = Guid.Parse("b0cb9253-4a21-4171-9b2a-5bfb1e27e992"),
                    Year = 2015,
                    EntityStatus = YourEntityStatus.Status1
                }
            };

            await _context.YourEntities.AddRangeAsync(newEntities);
        }
        
        await _context.SaveChangesAsync();
    }
}