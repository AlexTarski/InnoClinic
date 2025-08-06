using InnoClinic.Authorization.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.Infrastructure;

public class AuthorizationContext : IdentityDbContext<YourEntity,
    IdentityRole<Guid>,
    Guid,
    IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
{
    public DbSet<YourEntity> YourEntities { get; set; }

    public AuthorizationContext()
    {
    }

    public AuthorizationContext(DbContextOptions<AuthorizationContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<YourEntity>(entity => entity.ToTable(name: "YourEntities"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<Guid>>(entity =>
            entity.ToTable(name: "UserRoles"));
        builder.Entity<IdentityUserClaim<Guid>>(entity =>
            entity.ToTable(name: "UserClaim"));
        builder.Entity<IdentityUserLogin<Guid>>(entity =>
            entity.ToTable("UserLogins"));
        builder.Entity<IdentityUserToken<Guid>>(entity =>
            entity.ToTable("UserTokens"));
        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
            entity.ToTable("RoleClaims"));

        builder.ApplyConfiguration(new YourEntityConfiguration());
    }
}