using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal abstract class BaseIdEntity : BaseEntity
    {
        public Guid Id { get; set; }
    }
}