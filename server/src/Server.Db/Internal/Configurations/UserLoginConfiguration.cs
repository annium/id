using System;
using Annium.linq2db.Extensions.Configuration;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class UserLoginConfiguration : IdEntityConfiguration<UserLogin, Guid>
{
    public override void Configure(EntityMappingBuilder<UserLogin> builder)
    {
        builder.HasSchemaName(Constants.Schema).HasTableName("user_logins");
        base.Configure(builder);

        builder.Association(x => x.User, x => x.UserId, x => x.Id, false);
        builder.Association(x => x.App, x => x.AppId, x => x.Id, false);
        builder.Property(x => x.LoggedAt).IsColumn();
        builder.Property(x => x.IpAddress).IsColumn();
        builder.Property(x => x.Client).IsColumn();
        builder.Property(x => x.RefreshToken).IsColumn();
        builder.Property(x => x.RefreshTokenExpires).IsColumn();
    }
}