using InnoClinic.Profiles.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Profiles.Infrastructure;

public class AccountsRepository : BaseCrudRepository<Account>
{
    public AccountsRepository(ProfilesContext context) 
    : base(context) { }
}