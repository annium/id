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

        public virtual DbSet<Company> CompaniesSet { get; set; }

        public ITable<Company> Companies => CompaniesSet.ToLinqToDBTable();

        public virtual DbSet<CompanyClaim> CompanyClaimsSet { get; set; }

        public ITable<CompanyClaim> CompanyClaims => CompanyClaimsSet.ToLinqToDBTable();

        public virtual DbSet<CompanyRole> CompanyRolesSet { get; set; }

        public ITable<CompanyRole> CompanyRoles => CompanyRolesSet.ToLinqToDBTable();

        public virtual DbSet<CompanyRoleClaim> CompanyRoleClaimsSet { get; set; }

        public ITable<CompanyRoleClaim> CompanyRoleClaims => CompanyRoleClaimsSet.ToLinqToDBTable();

        public virtual DbSet<CompanyUser> CompanyUsersSet { get; set; }

        public ITable<CompanyUser> CompanyUsers => CompanyUsersSet.ToLinqToDBTable();

        public virtual DbSet<CompanyUserClaim> CompanyUserClaimsSet { get; set; }

        public ITable<CompanyUserClaim> CompanyUserClaims => CompanyUserClaimsSet.ToLinqToDBTable();

        public virtual DbSet<CompanyUserRole> CompanyUserRolesSet { get; set; }

        public ITable<CompanyUserRole> CompanyUserRoles => CompanyUserRolesSet.ToLinqToDBTable();

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

            builder.Entity<Company>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Company>()
                .HasOne<Company>().WithMany()
                .HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Company>()
                .HasAlternateKey(m => m.Key);

            builder.Entity<CompanyClaim>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyClaim>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<CompanyRole>()
                .HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(m => m.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyRole>()
                .HasAlternateKey(m => new { m.AppId, m.Key });

            builder.Entity<CompanyRoleClaim>()
                .HasKey(p => new { p.RoleId, p.ClaimId });
            builder.Entity<CompanyRoleClaim>()
                .HasOne<CompanyRole>().WithMany().IsRequired()
                .HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyRoleClaim>()
                .HasOne<CompanyClaim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CompanyUser>()
                .HasKey(p => new { p.CompanyId, p.UserId });
            builder.Entity<CompanyUser>()
                .HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(m => m.CompanyId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyUser>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CompanyUserClaim>()
                .HasKey(p => new { p.CompanyId, p.UserId, p.ClaimId });
            builder.Entity<CompanyUserClaim>()
                .HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(m => m.CompanyId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyUserClaim>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyUserClaim>()
                .HasOne<CompanyClaim>().WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CompanyUserRole>()
                .HasKey(p => new { p.CompanyId, p.UserId, p.RoleId });
            builder.Entity<CompanyUserRole>()
                .HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(m => m.CompanyId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyUserRole>()
                .HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CompanyUserRole>()
                .HasOne<CompanyRole>().WithMany().IsRequired()
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