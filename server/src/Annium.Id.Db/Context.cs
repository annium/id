using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db.Entities
{
    internal class Context : DbContext, IContext
    {
        public virtual DbSet<App> AppsSet { get; set; }

        public ITable<App> Apps => AppsSet.ToLinqToDBTable();

        public virtual DbSet<Claim> ClaimsSet { get; set; }

        public ITable<Claim> Claims => ClaimsSet.ToLinqToDBTable();

        public virtual DbSet<Organization> OrganizationsSet { get; set; }

        public ITable<Organization> Organizations => OrganizationsSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationClaim> OrganizationClaimsSet { get; set; }

        public ITable<OrganizationClaim> OrganizationClaims => OrganizationClaimsSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationRole> OrganizationRolesSet { get; set; }

        public ITable<OrganizationRole> OrganizationRoles => OrganizationRolesSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationRoleClaim> OrganizationRoleClaimsSet { get; set; }

        public ITable<OrganizationRoleClaim> OrganizationRoleClaims => OrganizationRoleClaimsSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationUser> OrganizationUsersSet { get; set; }

        public ITable<OrganizationUser> OrganizationUsers => OrganizationUsersSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationUserClaim> OrganizationUserClaimsSet { get; set; }

        public ITable<OrganizationUserClaim> OrganizationUserClaims => OrganizationUserClaimsSet.ToLinqToDBTable();

        public virtual DbSet<OrganizationUserRole> OrganizationUserRolesSet { get; set; }

        public ITable<OrganizationUserRole> OrganizationUserRoles => OrganizationUserRolesSet.ToLinqToDBTable();

        public virtual DbSet<Role> RolesSet { get; set; }

        public ITable<Role> Roles => RolesSet.ToLinqToDBTable();

        public virtual DbSet<RoleClaim> RoleClaimsSet { get; set; }

        public ITable<RoleClaim> RoleClaims => RoleClaimsSet.ToLinqToDBTable();

        public virtual DbSet<User> UsersSet { get; set; }

        public ITable<User> Users => UsersSet.ToLinqToDBTable();

        public virtual DbSet<UserClaim> UserClaimsSet { get; set; }

        public ITable<UserClaim> UserClaims => UserClaimsSet.ToLinqToDBTable();

        public virtual DbSet<UserLogin> UserLoginsSet { get; set; }

        public ITable<UserLogin> UserLogins => UserLoginsSet.ToLinqToDBTable();

        public virtual DbSet<UserRole> UserRolesSet { get; set; }

        public ITable<UserRole> UserRoles => UserRolesSet.ToLinqToDBTable();

        public Context(DbContextOptions contextOptions) : base(contextOptions) { }

        public DataConnection GetDataConnection() => this.CreateLinqToDbConnection();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<App>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<App>()
                .HasAlternateKey(m => m.Key);

            builder.Entity<Claim>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Claim>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<Organization>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Organization>()
                .HasOne<Organization>().WithMany()
                .HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Organization>()
                .HasAlternateKey(m => m.Key);

            builder.Entity<OrganizationClaim>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationClaim>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<OrganizationRole>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationRole>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<OrganizationRoleClaim>()
                .HasKey(p => new { p.RoleId, p.ClaimId });
            builder.Entity<OrganizationRoleClaim>()
                .HasOne<OrganizationRole>().WithMany().IsRequired()
                .HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationRoleClaim>()
                .HasOne<OrganizationClaim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrganizationUser>()
                .HasKey(p => new { p.OrganizationId, p.UserId });
            builder.Entity<OrganizationUser>()
                .HasOne<Organization>().WithMany().IsRequired()
                .HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationUser>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrganizationUserClaim>()
                .HasKey(p => new { p.OrganizationId, p.UserId, p.ClaimId });
            builder.Entity<OrganizationUserClaim>()
                .HasOne<Organization>().WithMany().IsRequired()
                .HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationUserClaim>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationUserClaim>()
                .HasOne<OrganizationClaim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrganizationUserRole>()
                .HasKey(p => new { p.OrganizationId, p.UserId, p.RoleId });
            builder.Entity<OrganizationUserRole>()
                .HasOne<Organization>().WithMany().IsRequired()
                .HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationUserRole>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<OrganizationUserRole>()
                .HasOne<OrganizationRole>().WithMany().IsRequired()
                .HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Role>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Role>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<RoleClaim>()
                .HasKey(p => new { p.RoleId, p.ClaimId });
            builder.Entity<RoleClaim>()
                .HasOne<Role>().WithMany().IsRequired()
                .HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<RoleClaim>()
                .HasOne<Claim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasAlternateKey(m => m.Login);
            builder.Entity<User>()
                .HasAlternateKey(m => m.Email);

            builder.Entity<UserClaim>()
                .HasKey(p => new { p.UserId, p.ClaimId });
            builder.Entity<UserClaim>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserClaim>()
                .HasOne<Claim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserLogin>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserLogin>()
                .HasAlternateKey(m => m.RefreshToken);

            builder.Entity<UserRole>()
                .HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<UserRole>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserRole>()
                .HasOne<Role>().WithMany().IsRequired()
                .HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}