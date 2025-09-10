using InnoClinic.Authorization.Domain.Entities.Users;
using InnoClinic.Shared;
using InnoClinic.Shared.DataSeeding;
using InnoClinic.Shared.DataSeeding.Entities;
using InnoClinic.Shared.DataSeeding.Entities.ProfileTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.Infrastructure.DataSeeders
{
    public class AccountsDataSeeder
    {
        private readonly AuthorizationContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AccountsDataSeeder(AuthorizationContext context,
            UserManager<Account> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            if (!await _context.Accounts.AnyAsync())
            {
                await SeedRoles();
                var accounts = new List<Account>
                {
                    #region Doctors
                    await CreateDoctor(Guid.Parse("f3d8d926-3e40-4a1f-bc84-2ddf7b72e381")),
                    await CreateDoctor(Guid.Parse("a9e25ad4-cc35-4f12-b0de-17fe1c5b7397")),
                    await CreateDoctor(Guid.Parse("5087df3a-a3df-4ea6-bc42-b635643b7cf0")),
                    #endregion
                    #region Receptionists
                    await CreateReceptionist(Guid.Parse("8a9cdb10-b244-4719-bc49-6a74c187dac5")),
                    await CreateReceptionist(Guid.Parse("31a8ee45-f69d-4a46-a3af-e14d857493d6")),
                    await CreateReceptionist(Guid.Parse("f4de03f6-700f-4c94-aea2-034fc56738e7")),
                    #endregion
                    #region Patients
                    await CreatePatient(Guid.Parse("c8a5b172-0c91-413e-87c0-559e58af8107")),
                    await CreatePatient(Guid.Parse("7bc0dbde-f9b4-4b91-9b81-1e534908360f")),
                    await CreatePatient(Guid.Parse("e6d391d8-632c-4e6d-b524-8f467d9a44c2"))
                    #endregion
                };
                
                await _context.Accounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();
            }
        }

        private static Account CreateAccount<T>(Guid id, Account<T> accountData)
            where T : User
        {
            var newAccount = new Account()
            {
                Id = id,
                UserName = accountData.Email,
                NormalizedUserName = accountData.Email.ToUpperInvariant(),
                Email = accountData.Email,
                NormalizedEmail = accountData.Email.ToUpperInvariant(),
                PasswordHash = accountData.PasswordHash,
                SecurityStamp = Guid.NewGuid().ToString("N").ToUpperInvariant(),
                PhoneNumber = accountData.PhoneNumber,
                EmailConfirmed = true,
                PhotoId = accountData.PhotoId,
                CreatedBy = id,
                CreatedAt = DateTime.UtcNow.AddDays(-365),
                UpdatedBy = id,
                UpdatedAt = DateTime.UtcNow
            };

            return newAccount;
        }

        private async Task<Account> CreateDoctor(Guid id)
        {
            var accountData = SampleData.Doctors[id];
            var newAccount =  CreateAccount(id, accountData);
            await AssignToRole(newAccount, ProfileType.Doctor.ToString());
            return newAccount;
        }

        private async Task<Account> CreateReceptionist(Guid id)
        {
            var accountData = SampleData.Receptionists[id];
            var newAccount = CreateAccount(id, accountData);
            await AssignToRole(newAccount, ProfileType.Receptionist.ToString());
            return newAccount;
        }

        private async Task<Account> CreatePatient(Guid id)
        {
            var accountData = SampleData.Patients[id];
            var newAccount = CreateAccount(id, accountData);
            await AssignToRole(newAccount, ProfileType.Patient.ToString());
            return newAccount;
        }

        private async Task AssignToRole(Account user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        private async Task SeedRoles()
        {
            string[] roles = Enum.GetNames<ProfileType>();
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}