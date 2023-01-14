using System;
using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyConfiguration : IdEntityConfiguration<Company, Guid>
{
    public override void Configure(EntityMappingBuilder<Company> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("companies");
        base.Configure(builder);
        builder.Association(x => x.Owner, x => x.OwnerId, x => x.Id, false);
        builder.Association(x => x.Parent, x => x.ParentId, x => x!.Id);
        builder.Property(x => x.Name).IsColumn();
    }
}