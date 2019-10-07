using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class RoleClaimConfiguration : BaseEntityConfiguration<RoleClaim>
    {
        public override void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasKey(p => new { p.RoleId, p.ClaimId });
            builder.HasOne(x => x.Role).WithMany(x => x.Claims).IsRequired()
                .HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Claim).WithMany().IsRequired()
                .HasForeignKey(x => x.ClaimId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}