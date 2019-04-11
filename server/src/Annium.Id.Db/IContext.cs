using LinqToDB;
using LinqToDB.Data;

namespace Annium.Id.Db.Entities
{
    internal interface IContext
    {
        ITable<App> Apps { get; }

        ITable<Claim> Claims { get; }

        ITable<Organization> Organizations { get; }

        ITable<OrganizationClaim> OrganizationClaims { get; }

        ITable<OrganizationRole> OrganizationRoles { get; }

        ITable<OrganizationRoleClaim> OrganizationRoleClaims { get; }

        ITable<OrganizationUser> OrganizationUsers { get; }

        ITable<OrganizationUserClaim> OrganizationUserClaims { get; }

        ITable<OrganizationUserRole> OrganizationUserRoles { get; }

        ITable<Role> Roles { get; }

        ITable<RoleClaim> RoleClaims { get; }

        ITable<User> Users { get; }

        ITable<UserClaim> UserClaims { get; }

        ITable<UserLogin> UserLogins { get; }

        ITable<UserRole> UserRoles { get; }

        DataConnection GetDataConnection();
    }
}