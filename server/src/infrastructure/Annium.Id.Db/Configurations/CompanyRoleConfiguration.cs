using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class CompanyRoleConfiguration : BaseIdEntityConfiguration<CompanyRole>
    {
        public override void Configure(EntityTypeBuilder<CompanyRole> builder)
        {
            base.Configure(builder);

            builder.HasIndex(m => new { m.AppId, m.Key }).IsUnique();

            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Claims);
        }
    }
}