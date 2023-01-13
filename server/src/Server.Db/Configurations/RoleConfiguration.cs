using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal class RoleConfiguration : BaseIdEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.HasOne<App>().WithMany().IsRequired()
            .HasForeignKey(x => x.AppId);
        builder.HasIndex(m => new { m.AppId, m.Key }).IsUnique();
    }
}