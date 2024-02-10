using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyClaimConfiguration : IIdEntityConfiguration<CompanyClaim, Guid>
{
    public void Configure(EntityMappingBuilder<CompanyClaim> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("company_claims");
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.Key).IsColumn();
        builder.Property(x => x.Name).IsColumn();
    }
}
