using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyUserClaimConfiguration : IEntityConfiguration<CompanyUserClaim>
{
    public void Configure(EntityMappingBuilder<CompanyUserClaim> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("company_user_claims");
        builder.HasPrimaryKey(x => new { x.CompanyId, x.UserId, x.ClaimId });
        builder.Association(x => x.Company, x => x.CompanyId, x => x.Id, false);
        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
        builder.Association(x => x.Claim, x => x.ClaimId, x => x.Id, false);
        builder.Property(x => x.Value).IsColumn();
    }
}