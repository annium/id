using Annium.Id.Infrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Infrastructure.Db.Configurations
{
    internal class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(p => new { p.UserId, p.RoleId });
            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Role).WithMany().IsRequired()
                .HasForeignKey(x => x.RoleId);
        }
    }
}