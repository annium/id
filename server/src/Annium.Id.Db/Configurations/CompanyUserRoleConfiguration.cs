using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class CompanyUserRoleConfiguration : BaseEntityConfiguration<CompanyUserRole>
    {
        public override void Configure(EntityTypeBuilder<CompanyUserRole> builder)
        {
            builder.HasKey(p => new { p.CompanyId, p.UserId, p.RoleId });
            builder.HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Role).WithMany().IsRequired()
                .HasForeignKey(x => x.RoleId).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}