using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal class AppConfiguration : BaseIdEntityConfiguration<App>
{
    public override void Configure(EntityTypeBuilder<App> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Owner).WithMany().IsRequired()
            .HasForeignKey(m => m.OwnerId);
    }
}