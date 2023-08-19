using Annium.Logging.Abstractions;
using LinqToDB;
using LinqToDB.Data;
using Server.Domain.Models;

namespace Server.Db;

public class ServerConnection : DataConnection, ILogSubject<ServerConnection>
{
    public ILogger<ServerConnection> Logger { get; }
    public ITable<App> Apps { get; set; }
    public ITable<Claim> Claims { get; set; }
    public ITable<Company> Companies { get; set; }
    public ITable<CompanyClaim> CompanyClaims { get; set; }
    public ITable<CompanyRole> CompanyRoles { get; set; }
    public ITable<CompanyRoleClaim> CompanyRoleClaims { get; set; }
    public ITable<CompanyUser> CompanyUsers { get; set; }
    public ITable<CompanyUserClaim> CompanyUserClaims { get; set; }
    public ITable<CompanyUserRole> CompanyUserRoles { get; set; }
    public ITable<Role> Roles { get; set; }
    public ITable<RoleClaim> RoleClaims { get; set; }
    public ITable<User> Users { get; set; }
    public ITable<UserClaim> UserClaims { get; set; }
    public ITable<UserLogin> UserLogins { get; set; }
    public ITable<UserRole> UserRoles { get; set; }

    public ServerConnection(
        DataOptions<ServerConnection> config,
        ILogger<ServerConnection> logger
    ) : base(config.Options)
    {
        Logger = logger;
        Apps = this.GetTable<App>();
        Claims = this.GetTable<Claim>();
        Companies = this.GetTable<Company>();
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