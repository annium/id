using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyRoleConfiguration : IIdEntityConfiguration<CompanyRole, Guid>
{
    public void Configure(EntityMappingBuilder<CompanyRole> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("company_roles");
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Association(x => x.Claims, x => x.Id, x => x.RoleId, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
    }
}
