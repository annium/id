using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class UserLoginConfiguration : BaseIdEntityConfiguration<UserLogin>
    {
        public override void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId);
            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId);
            builder.HasIndex(m => m.RefreshToken).IsUnique();
        }
    }
}