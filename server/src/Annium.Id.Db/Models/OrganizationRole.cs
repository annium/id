using System;

namespace Annium.Id.Db
{
    public class OrganizationRole
    {
        public Guid Id { get; }

        public Guid AppId { get; }

        public string Key { get; set; }

        public string Name { get; set; }

        public OrganizationRole(
            Guid appId,
            string key,
            string name
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
        }

        internal OrganizationRole(
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