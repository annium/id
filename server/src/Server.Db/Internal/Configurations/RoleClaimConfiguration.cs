using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class RoleClaimConfiguration : IEntityConfiguration<RoleClaim>
{
    public void Configure(EntityMappingBuilder<RoleClaim> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("role_claims");
        builder.HasPrimaryKey(x => new { x.RoleId, x.ClaimId });
        builder.Association(x => x.Role, x => x.RoleId, x => x.Id, false);
        builder.Association(x => x.Claim, x => x.ClaimId, x => x.Id, false);
        builder.Property(x => x.Value).IsColumn();
    }
}