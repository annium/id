using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class UserLoginConfiguration : BaseIdEntityConfiguration<UserLogin>
    {
        public override void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<App>().WithMany().IsRequired()
                .HasForeignKey(x => x.AppId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(m => m.RefreshToken).IsUnique();
        }
    }
}