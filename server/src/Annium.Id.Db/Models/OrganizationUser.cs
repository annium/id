using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class OrganizationUser
    {
        public Guid OrganizationId { get; }

        public Guid UserId { get; }

        [JsonConstructor]
        internal OrganizationUser(
            Guid organizationId,
            Guid userId
        )
        {
            OrganizationId = organizationId;
            UserId = userId;
        }
    }
}