using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Infrastructure.Repositories;

public class YourEntityRepository : BaseCrudRepository<Account>
{
    public YourEntityRepository(AuthorizationContext context)
    : base(context) { }
}