using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class UserRoleConfiguration : IEntityConfiguration<UserRole>
{
    public void Configure(EntityMappingBuilder<UserRole> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("user_roles");
        builder.HasPrimaryKey(x => new { x.UserId, x.RoleId });
        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
        builder.Association(x => x.Role, x => x.RoleId, x => x.Id, false);
    }
}