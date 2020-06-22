using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class CompanyUserRoleConfiguration : BaseEntityConfiguration<CompanyUserRole>
    {
        public override void Configure(EntityTypeBuilder<CompanyUserRole> builder)
        {
            builder.HasKey(p => new { p.CompanyId, p.UserId, p.RoleId });
            builder.HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(x => x.CompanyId);
            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Role).WithMany().IsRequired()
                .HasForeignKey(x => x.RoleId);
        }
    }
}