using Annium.linq2db.Extensions.Models;
using Annium.Logging.Abstractions;
using LinqToDB;
using Server.Domain.Models;

namespace Server.Db;

public class ServerConnection : DataConnectionBase, ILogSubject<ServerConnection>
{
    public ILogger<ServerConnection> Logger { get; }
    public ITable<App> Apps { get; set; } = null!;
    public ITable<Claim> Claims { get; set; } = null!;
    public ITable<Company> Companies { get; set; } = null!;
    public ITable<CompanyClaim> CompanyClaims { get; set; } = null!;
    public ITable<CompanyRole> CompanyRoles { get; set; } = null!;
    public ITable<CompanyRoleClaim> CompanyRoleClaims { get; set; } = null!;
    public ITable<CompanyUser> CompanyUsers { get; set; } = null!;
    public ITable<CompanyUserClaim> CompanyUserClaims { get; set; } = null!;
    public ITable<CompanyUserRole> CompanyUserRoles { get; set; } = null!;
    public ITable<Role> Roles { get; set; } = null!;
    public ITable<RoleClaim> RoleClaims { get; set; } = null!;
    public ITable<User> Users { get; set; } = null!;
    public ITable<UserClaim> UserClaims { get; set; } = null!;
    public ITable<UserLogin> UserLogins { get; set; } = null!;
    public ITable<UserRole> UserRoles { get; set; } = null!;

    public ServerConnection(
        Config<ServerConnection> config,
        ILogger<ServerConnection> logger
    ) : base(config.Options)
    {
        Logger = logger;
        Apps = this.GetTable<App>();
        Claims = this.GetTable<Claim>();
        CompanyClaims = this.GetTable<CompanyClaim>();
        CompanyRoles = this.GetTable<CompanyRole>();
        CompanyRoleClaims = this.GetTable<CompanyRoleClaim>();
        CompanyUsers = this.GetTable<CompanyUser>();
        CompanyUserClaims = this.GetTable<CompanyUserClaim>();
        CompanyUserRoles = this.GetTable<CompanyUserRole>();
        Roles = this.GetTable<Role>();
        RoleClaims = this.GetTable<RoleClaim>();
        Users = this.GetTable<User>();
        UserClaims = this.GetTable<UserClaim>();
        UserLogins = this.GetTable<UserLogin>();
        UserRoles = this.GetTable<UserRole>();
    }
}