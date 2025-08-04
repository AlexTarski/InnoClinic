using InnoClinic.Authorization.Domain.Entities.Users;

namespace InnoClinic.Authorization.Infrastructure.Repositories;

public class YourEntityRepository : BaseCrudRepository<YourEntity>
{
    public YourEntityRepository(AuthorizationContext context)
    : base(context) { }
}