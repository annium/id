using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class CompanyUserRoleConfiguration : IEntityConfiguration<CompanyUserRole>
{
    public void Configure(EntityMappingBuilder<CompanyUserRole> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("company_user_roles");
        builder.HasPrimaryKey(x => new
        {
            x.CompanyId,
            x.UserId,
            x.RoleId
        });
        builder.Association(x => x.Company, x => x.CompanyId, x => x.Id, false);
        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
        builder.Association(x => x.Role, x => x.RoleId, x => x.Id, false);
    }
}
