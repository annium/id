using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class CompanyRoleConfiguration : BaseIdEntityConfiguration<CompanyRole>
    {
        public override void Configure(EntityTypeBuilder<CompanyRole> builder)
        {
            base.Configure(builder);

            builder.HasIndex(m => new { m.AppId, m.Key }).IsUnique();

            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId);
            builder.HasMany(x => x.Claims);
        }
    }
}