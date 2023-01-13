using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Db.Entities;

namespace Server.Db.Configurations;

internal abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public abstract void Configure(EntityTypeBuilder<TEntity> builder);
}