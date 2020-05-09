using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class UserConfiguration : BaseIdEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasIndex(m => m.Login).IsUnique();
            builder.HasIndex(m => m.Email).IsUnique();
        }
    }
}