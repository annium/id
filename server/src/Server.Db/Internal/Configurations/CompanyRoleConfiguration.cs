using System;
using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyRoleConfiguration : IdEntityConfiguration<CompanyRole, Guid>
{
    public override void Configure(EntityMappingBuilder<CompanyRole> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("company_roles");
        base.Configure(builder);
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Association(x => x.Claims, x => x.Id, x => x.RoleId, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
    }
}