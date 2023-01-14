using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class UserClaimConfiguration : IEntityConfiguration<UserClaim>
{
    public void Configure(EntityMappingBuilder<UserClaim> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("user_claims");
        builder.HasPrimaryKey(x => new { x.UserId, x.ClaimId });
        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
        builder.Association(x => x.Claim, x => x.ClaimId, x => x.Id, false);
        builder.Property(x => x.Value).IsColumn();
    }
}