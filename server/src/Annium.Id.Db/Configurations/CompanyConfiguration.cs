using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class CompanyConfiguration : BaseIdEntityConfiguration<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>().WithMany().IsRequired()
                .HasForeignKey(m => m.OwnerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Company>().WithMany()
                .HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(m => m.Key).IsUnique();
        }
    }
}