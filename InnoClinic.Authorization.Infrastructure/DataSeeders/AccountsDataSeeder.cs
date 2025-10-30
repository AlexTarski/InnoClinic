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
                    await CreateDoctor(SampleData.ElenaVolkova),
                    await CreateDoctor(SampleData.SergeyIvanov),
                    await CreateDoctor(SampleData.AminaSadikova),
                    await CreateDoctor(SampleData.IvanPetrov),
                    await CreateDoctor(SampleData.LucasMartinez),
                    await CreateDoctor(SampleData.SophiaChen),
                    #endregion
                    #region Receptionists
                    await CreateReceptionist(SampleData.OlgaSmirnova),
                    await CreateReceptionist(SampleData.MateuszKowalski),
                    await CreateReceptionist(SampleData.LeylaAbdulova),
                    #endregion
                    #region Patients
                    await CreatePatient(SampleData.MaximPetrov),
                    await CreatePatient(SampleData.CharlotteBergman),
                    await CreatePatient(SampleData.RajeshMehta)
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