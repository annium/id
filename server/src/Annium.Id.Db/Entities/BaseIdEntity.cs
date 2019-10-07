using System;

namespace Annium.Id.Db.Entities
{
    internal abstract class BaseIdEntity : BaseEntity
    {
        public Guid Id { get; set; }
    }
}