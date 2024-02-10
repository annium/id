using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class AppConfiguration : IIdEntityConfiguration<App, Guid>
{
    public void Configure(EntityMappingBuilder<App> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("apps");
        builder.Association(x => x.Owner, x => x.OwnerId, x => x.Id, false);
        builder.Property(x => x.Name).IsColumn();
        builder.Property(x => x.ApiToken).IsColumn();
    }
}
