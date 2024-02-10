using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class RoleConfiguration : IIdEntityConfiguration<Role, Guid>
{
    public void Configure(EntityMappingBuilder<Role> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("roles");
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
        builder.Association(x => x.Claims, x => x.Id, x => x.RoleId, false);
    }
}
