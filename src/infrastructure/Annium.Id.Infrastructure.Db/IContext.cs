using System.Threading;
using System.Threading.Tasks;
using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Infrastructure.Db;

internal interface IContext
{
    DbSet<App> Apps { get; }
    DbSet<Claim> Claims { get; }
    DbSet<Company> Companies { get; }
    DbSet<CompanyClaim> CompanyClaims { get; }
    DbSet<CompanyRole> CompanyRoles { get; }
    DbSet<CompanyRoleClaim> CompanyRoleClaims { get; }
    DbSet<CompanyUser> CompanyUsers { get; }
    DbSet<CompanyUserClaim> CompanyUserClaims { get; }
    DbSet<CompanyUserRole> CompanyUserRoles { get; }
    DbSet<Role> Roles { get; }
    DbSet<RoleClaim> RoleClaims { get; }
    DbSet<User> Users { get; }
    DbSet<UserClaim> UserClaims { get; }
    DbSet<UserLogin> UserLogins { get; }
    DbSet<UserRole> UserRoles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}