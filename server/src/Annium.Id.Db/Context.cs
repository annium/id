using System;
using Annium.Id.Core;
using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Annium.Id.Db
{
    internal class Context : DbContext, IContext
    {
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<Claim> Claims { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<CompanyClaim> CompanyClaims { get; set; } = null!;
        public DbSet<CompanyRole> CompanyRoles { get; set; } = null!;
        public DbSet<CompanyRoleClaim> CompanyRoleClaims { get; set; } = null!;
        public DbSet<CompanyUser> CompanyUsers { get; set; } = null!;
        public DbSet<CompanyUserClaim> CompanyUserClaims { get; set; } = null!;
        public DbSet<CompanyUserRole> CompanyUserRoles { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RoleClaim> RoleClaims { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserClaim> UserClaims { get; set; } = null!;
        public DbSet<UserLogin> UserLogins { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        public Context(DbContextOptions contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            builder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("baa0ad0f-91c5-4c19-963c-ea369048e67a"),
                Login = "alex",
                PasswordHash = "ohraPG8QMZiOnXX+MWh/45aZDwjtv/7FQMFzXxSRxQjLdSMBHpELKDSznF6cSUalufovlgCfFkn4mtR7eXB+8w==",
                Email = "a.kreskiyan@gmail.com",
            });

            builder.Entity<App>().HasData(new App
            {
                Id = Constants.IdAppId,
               Name = "Annium ID",
                OwnerId = Guid.Parse("baa0ad0f-91c5-4c19-963c-ea369048e67a"),
                ApiToken = Guid.Parse("b62acd2a-2f1b-4da1-9273-abab4b9da7f7"),
            });
        }
    }
}