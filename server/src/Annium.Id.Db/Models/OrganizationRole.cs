using System;
using System.Collections.Generic;

namespace Annium.Id.Db
{
    public class OrganizationRole
    {
        public Guid Id { get; }

        public Guid AppId { get; }

        public string Key { get; set; }

        public string Name { get; set; }

        public IEnumerable<ClaimValue> Claims { get; }

        public OrganizationRole(
            Guid appId,
            string key,
            string name,
            IEnumerable<ClaimValue> claims
        )
        {
            AppId = appId;
            Key = key;
            Name = name;
            Claims = claims;
        }

        internal OrganizationRole(
            Guid id,
            Guid appId,
            string key,
            string name,
            IEnumerable<ClaimValue> claims
        ) : this(appId, key, name, claims)
        {
            Id = id;
        }
    }
}