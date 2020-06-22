using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class CompanyUserClaimConfiguration : BaseEntityConfiguration<CompanyUserClaim>
    {
        public override void Configure(EntityTypeBuilder<CompanyUserClaim> builder)
        {
            builder.HasKey(p => new { p.CompanyId, p.UserId, p.ClaimId });
            builder.HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(x => x.CompanyId);
            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Claim).WithMany().IsRequired()
                .HasForeignKey(m => m.ClaimId);
        }
    }
}