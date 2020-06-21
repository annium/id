using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class UserConfiguration : BaseIdEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>().WithMany()
                .HasForeignKey(m => m.ReferralId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => m.Login).IsUnique();
            builder.HasIndex(m => m.Email).IsUnique();
        }
    }
}