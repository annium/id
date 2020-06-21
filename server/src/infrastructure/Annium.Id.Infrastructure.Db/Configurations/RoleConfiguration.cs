using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class RoleConfiguration : BaseIdEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(m => new { m.AppId, m.Key }).IsUnique();
        }
    }
}