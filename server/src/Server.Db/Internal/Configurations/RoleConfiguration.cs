using System;
using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class RoleConfiguration : IdEntityConfiguration<Role, Guid>
{
    public override void Configure(EntityMappingBuilder<Role> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("roles");
        base.Configure(builder);
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
    }
}