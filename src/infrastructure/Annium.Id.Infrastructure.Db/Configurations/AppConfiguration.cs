using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations;

internal class AppConfiguration : BaseIdEntityConfiguration<App>
{
    public override void Configure(EntityTypeBuilder<App> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Owner).WithMany().IsRequired()
            .HasForeignKey(m => m.OwnerId);
    }
}