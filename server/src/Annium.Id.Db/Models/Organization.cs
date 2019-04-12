using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class Organization
    {
        public Guid Id { get; }

        public Guid OwnerId { get; }

        public Guid ParentId { get; }

        public string Key { get; set; }

        public string Name { get; set; }

        public Organization(
            Guid ownerId,
            Guid parentId,
            string key,
            string name
        )
        {
            OwnerId = ownerId;
            ParentId = parentId;
            Key = key;
            Name = name;
        }

        [JsonConstructor]
        internal Organization(
            Guid id,
            Guid ownerId,
            Guid parentId,
            string login,
            string name
        ) : this(ownerId, parentId, login, name)
        {
            Id = id;
        }
    }
}