using System;

namespace Annium.Id.Domain.Entities
{
    public class CompanyClaim
    {
        public Guid Id { get; }
        public Guid AppId { get; }
        public string Key { get; set; }
        public string Name { get; set; }

        public CompanyClaim(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }

        internal CompanyClaim(
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