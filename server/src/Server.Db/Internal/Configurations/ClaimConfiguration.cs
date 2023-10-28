using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class ClaimConfiguration : IdEntityConfiguration<Claim, Guid>
{
    public override void Configure(EntityMappingBuilder<Claim> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("claims");
        base.Configure(builder);
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
    }
}
