using System;

namespace Annium.Id.Db
{
    public class Organization
    {
        public Guid Id { get; }

        public Guid OwnerId { get; set; }

        public Guid? ParentId { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public Organization(
            Guid ownerId,
            Guid? parentId,
            string key,
            string name
        )
        {
            OwnerId = ownerId;
            ParentId = parentId;
            Key = key;
            Name = name;
        }

        internal Organization(
            Guid id,
            Guid ownerId,
            Guid? parentId,
            string key,
            string name
        ) : this(ownerId, parentId, key, name)
        {
            Id = id;
        }
    }
}