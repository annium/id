using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
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
}