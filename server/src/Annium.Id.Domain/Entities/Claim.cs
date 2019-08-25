using System;

namespace Annium.Id.Domain.Entities
{
    public class Claim
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; set; }
        public string Name { get; set; }

        public Claim(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }

        internal Claim(
            Guid id,
            Guid appId,
            string key,
            string name
        ) : this(appId, key, name)
        {
            Id = id;
        }
    }
}