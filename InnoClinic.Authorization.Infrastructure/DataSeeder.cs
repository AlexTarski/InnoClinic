using Microsoft.EntityFrameworkCore;

using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Infrastructure
{
    public class DataSeeder
    {
        private readonly AuthorizationContext _context;

        public DataSeeder(AuthorizationContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!await _context.Accounts.AnyAsync())
            {
                var accounts = new List<Account>
                {
                    #region Doctors
                    DataSeeder.CreateAccount(
                        Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381"),
                        "elena.volkova@example.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+7-495-1234567"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397"),
                        "sergey.ivanov@example.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+7-812-9876543"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0"),
                        "amina.sadikova@example.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+7-383-4567890"),
                    #endregion
                    #region Receptionists
                    DataSeeder.CreateAccount(
                        Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5"),
                        "olga.smirnova@clinic.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+375-29-6543210"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6"),
                        "mateusz.kowalski@clinic.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+48-22-7891234"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7"),
                        "leyla.abdulova@clinic.com",
                        "AQAAAAIAAYagAAAAECvbTt2QtrwjwAUsgUxNRV8+M9Awq9jld0iZ+IL7XzTGd5k3A8S53jOS66nzyyFYAw==",
                        "+994-12-4567890"),
                    #endregion
                    #region Patients
                    DataSeeder.CreateAccount(
                        Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107"),
                        "maxim.petrov@patientmail.com",
                        "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                        "+7-911-1112222"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f"),
                        "charlotte.bergman@patientmail.com",
                        "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                        "+49-30-2223334"),
                    DataSeeder.CreateAccount(
                        Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2"),
                        "rajesh.mehta@patientmail.com",
                        "AQAAAAIAAYagAAAAEKAQ1M3rKsrLyON+SGi9ytnVLX+FP6XH6O92WamJqZ4UIwJhoEQwZRdr07xHquvJlg==",
                        "+91-22-4561230")
                    #endregion
                };
                
                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();
            }
        }

        private static Account CreateAccount(Guid id, string email, string passwordHash, string phoneNumber)
        {
            var newAccount = new Account()
            {
                Id = id,
                UserName = email,
                NormalizedUserName = email.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                PasswordHash = passwordHash,
                SecurityStamp = Guid.NewGuid().ToString("N").ToUpperInvariant(),
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                Photo_id = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow.AddDays(-365),
                UpdatedBy = Guid.NewGuid(),
                UpdatedAt = DateTime.UtcNow
            };

            return newAccount;
        }
    }
}