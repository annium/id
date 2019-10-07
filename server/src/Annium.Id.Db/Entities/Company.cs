using System;

namespace Annium.Id.Db.Entities
{
    internal class Company : BaseIdEntity
    {
        public Guid OwnerId { get; set; }
        public Guid? ParentId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}