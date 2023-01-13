using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal class RoleClaimConfiguration : BaseEntityConfiguration<RoleClaim>
{
    public override void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasKey(p => new { p.RoleId, p.ClaimId });
        builder.HasOne(x => x.Role).WithMany(x => x.Claims).IsRequired()
            .HasForeignKey(x => x.RoleId);
        builder.HasOne(x => x.Claim).WithMany().IsRequired()
            .HasForeignKey(x => x.ClaimId);
    }
}