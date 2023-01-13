using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal class CompanyUserConfiguration : BaseEntityConfiguration<CompanyUser>
{
    public override void Configure(EntityTypeBuilder<CompanyUser> builder)
    {
        builder.HasKey(p => new { p.CompanyId, p.UserId });
        builder.HasOne<Company>().WithMany().IsRequired()
            .HasForeignKey(x => x.CompanyId);
        builder.HasOne(x => x.User).WithMany().IsRequired()
            .HasForeignKey(x => x.UserId).HasForeignKey(x => x.UserId);
    }
}