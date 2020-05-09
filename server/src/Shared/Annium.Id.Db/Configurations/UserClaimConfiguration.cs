using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class UserClaimConfiguration : BaseEntityConfiguration<UserClaim>
    {
        public override void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasKey(p => new { p.UserId, p.ClaimId });
            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Claim).WithMany().IsRequired()
                .HasForeignKey(x => x.ClaimId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}