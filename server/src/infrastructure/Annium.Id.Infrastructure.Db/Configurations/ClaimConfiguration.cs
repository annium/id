using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class ClaimConfiguration : BaseIdEntityConfiguration<Claim>
    {
        public override void Configure(EntityTypeBuilder<Claim> builder)
        {
            base.Configure(builder);

            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId);
            builder.HasIndex(m => new { m.AppId, m.Key }).IsUnique();
        }
    }
}