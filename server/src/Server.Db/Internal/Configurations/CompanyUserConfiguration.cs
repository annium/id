using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyUserConfiguration : IEntityConfiguration<CompanyUser>
{
    public void Configure(EntityMappingBuilder<CompanyUser> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("company_users");
        builder.HasPrimaryKey(x => new { x.CompanyId, x.UserId });
        builder.Association(x => x.Company, x => x.CompanyId, x => x.Id, false);
        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
    }
}