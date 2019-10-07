using Annium.Id.Db.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Annium.Id.Db.Configurations
{
    internal abstract class BaseIdEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity> where TEntity : BaseIdEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}