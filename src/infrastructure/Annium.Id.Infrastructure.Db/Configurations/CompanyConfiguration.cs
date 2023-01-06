using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations;

internal class CompanyConfiguration : BaseIdEntityConfiguration<Company>
{
    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        base.Configure(builder);

        builder.HasOne<User>().WithMany().IsRequired()
            .HasForeignKey(m => m.OwnerId);
        builder.HasOne<Company>().WithMany()
            .HasForeignKey(m => m.ParentId);
    }
}