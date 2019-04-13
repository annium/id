using System;

namespace Annium.Id.Db
{
    public class OrganizationClaim
    {
        public Guid Id { get; }

        public Guid AppId { get; }

        public string Key { get; set; }

        public string Name { get; set; }

        public OrganizationClaim(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }

        internal OrganizationClaim(
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