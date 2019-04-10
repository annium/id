using LinqToDB;
using LinqToDB.Data;

namespace Annium.Id.Db
{
    internal interface IContext
    {
        ITable<Entities.App> Apps { get; }

        ITable<Entities.Claim> Claims { get; }

        ITable<Entities.Organization> Organizations { get; }

        ITable<Entities.OrganizationClaim> OrganizationClaims { get; }

        ITable<Entities.OrganizationRole> OrganizationRoles { get; }

        ITable<Entities.OrganizationRoleClaim> OrganizationRoleClaims { get; }

        ITable<Entities.OrganizationUser> OrganizationUsers { get; }

        ITable<Entities.OrganizationUserClaim> OrganizationUserClaims { get; }

        ITable<Entities.OrganizationUserRole> OrganizationUserRoles { get; }

        ITable<Entities.Role> Roles { get; }

        ITable<Entities.RoleClaim> RoleClaims { get; }

        ITable<Entities.User> Users { get; }

        ITable<Entities.UserClaim> UserClaims { get; }

        ITable<Entities.UserLogin> UserLogins { get; }

        ITable<Entities.UserRole> UserRoles { get; }

        DataConnection GetDataConnection();
    }
}