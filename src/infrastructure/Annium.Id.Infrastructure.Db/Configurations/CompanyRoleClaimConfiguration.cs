using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations;

internal class CompanyRoleClaimConfiguration : BaseEntityConfiguration<CompanyRoleClaim>
{
    public override void Configure(EntityTypeBuilder<CompanyRoleClaim> builder)
    {
        builder.HasKey(p => new { p.RoleId, p.ClaimId });
        builder.HasOne(x => x.Role).WithMany(x => x.Claims).IsRequired()
            .HasForeignKey(m => m.RoleId);
        builder.HasOne(x => x.Claim).WithMany().IsRequired()
            .HasForeignKey(m => m.ClaimId);
    }
}