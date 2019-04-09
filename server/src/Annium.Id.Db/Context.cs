using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db
{
    internal class Context : DbContext, IContext
    {
        public virtual DbSet<Entities.App> AppsSet { get; set; }

        public ITable<Entities.App> Apps => AppsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.Claim> ClaimsSet { get; set; }

        public ITable<Entities.Claim> Claims => ClaimsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.Organization> OrganizationsSet { get; set; }

        public ITable<Entities.Organization> Organizations => OrganizationsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.Role> RolesSet { get; set; }

        public ITable<Entities.Role> Roles => RolesSet.ToLinqToDBTable();

        public virtual DbSet<Entities.RoleClaim> RoleClaimsSet { get; set; }

        public ITable<Entities.RoleClaim> RoleClaims => RoleClaimsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.User> UsersSet { get; set; }

        public ITable<Entities.User> Users => UsersSet.ToLinqToDBTable();

        public virtual DbSet<Entities.UserClaim> UserClaimsSet { get; set; }

        public ITable<Entities.UserClaim> UserClaims => UserClaimsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.UserLogin> UserLoginsSet { get; set; }

        public ITable<Entities.UserLogin> UserLogins => UserLoginsSet.ToLinqToDBTable();

        public virtual DbSet<Entities.UserRole> UserRolesSet { get; set; }

        public ITable<Entities.UserRole> UserRoles => UserRolesSet.ToLinqToDBTable();

        public Context(DbContextOptions contextOptions) : base(contextOptions) { }

        public DataConnection GetDataConnection() => this.CreateLinqToDbConnection();

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}