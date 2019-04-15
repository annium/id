using LinqToDB;
using LinqToDB.Data;

namespace Annium.Id.Db.Entities
{
    internal interface IContext
    {
        ITable<App> Apps { get; }

        ITable<Claim> Claims { get; }

        ITable<Company> Companies { get; }

        ITable<CompanyClaim> CompanyClaims { get; }

        ITable<CompanyRole> CompanyRoles { get; }

        ITable<CompanyRoleClaim> CompanyRoleClaims { get; }

        ITable<CompanyUser> CompanyUsers { get; }

        ITable<CompanyUserClaim> CompanyUserClaims { get; }

        ITable<CompanyUserRole> CompanyUserRoles { get; }

        ITable<Role> Roles { get; }

        ITable<RoleClaim> RoleClaims { get; }

        ITable<User> Users { get; }

        ITable<UserClaim> UserClaims { get; }

        ITable<UserLogin> UserLogins { get; }

        ITable<UserRole> UserRoles { get; }

        DataConnection GetDataConnection();
    }
}