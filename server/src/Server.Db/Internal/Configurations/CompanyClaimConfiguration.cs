using System;
using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyClaimConfiguration : IdEntityConfiguration<CompanyClaim, Guid>
{
    public override void Configure(EntityMappingBuilder<CompanyClaim> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("company_claims");
        base.Configure(builder);
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.Name).IsColumn();
    }
}