using System;
using Annium.linq2db.Extensions;
using LinqToDB.Mapping;
using Server.Domain.Models;

namespace Server.Db.Internal.Configurations;

internal class UserConfiguration : IIdEntityConfiguration<User, Guid>
{
    public void Configure(EntityMappingBuilder<User> builder)
    {
        this.ConfigureId(builder);
        builder.HasSchemaName(Constants.Schema).HasTableName("users");
        builder.Property(x => x.Login).IsColumn();
        builder.Property(x => x.PasswordHash).IsColumn();
        builder.Property(x => x.Email).IsColumn();
        builder.Association(x => x.Referral, x => x.ReferralId, x => x!.Id);
    }
}
