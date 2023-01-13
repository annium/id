using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal class UserClaimConfiguration : BaseEntityConfiguration<UserClaim>
{
    public override void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasKey(p => new { p.UserId, p.ClaimId });
        builder.HasOne<User>().WithMany().IsRequired()
            .HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Claim).WithMany().IsRequired()
            .HasForeignKey(x => x.ClaimId);
    }
}