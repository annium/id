using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal class CompanyUserConfiguration : BaseEntityConfiguration<CompanyUser>
    {
        public override void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            builder.HasKey(p => new { p.CompanyId, p.UserId });
            builder.HasOne<Company>().WithMany().IsRequired()
                .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.User).WithMany().IsRequired()
                .HasForeignKey(x => x.UserId).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}