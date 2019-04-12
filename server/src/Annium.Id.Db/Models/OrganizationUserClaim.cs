using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class OrganizationUserClaim
    {
        public Guid OrganizationId { get; }

        public Guid UserId { get; }

        public Guid ClaimId { get; }

        public string Value { get; }

        [JsonConstructor]
        internal OrganizationUserClaim(
            Guid organizationId,
            Guid userId,
            Guid claimId,
            string value
        )
        {
            OrganizationId = organizationId;
            UserId = userId;
            ClaimId = claimId;
            Value = value;
        }
    }
}