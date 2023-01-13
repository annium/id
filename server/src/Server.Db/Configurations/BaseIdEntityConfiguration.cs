using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal abstract class BaseIdEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity> where TEntity : BaseIdEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}