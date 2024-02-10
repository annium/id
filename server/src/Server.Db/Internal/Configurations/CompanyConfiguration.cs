using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyConfiguration : IIdEntityConfiguration<Company, Guid>
{
    public void Configure(EntityMappingBuilder<Company> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("companies");
        builder.Association(x => x.Owner, x => x.OwnerId, x => x.Id, false);
        builder.Association(x => x.Parent, x => x.ParentId, x => x!.Id);
        builder.Property(x => x.Name).IsColumn();
    }
}
