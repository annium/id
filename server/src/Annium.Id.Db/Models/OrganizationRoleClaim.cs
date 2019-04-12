using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class OrganizationRoleClaim
    {
        public Guid RoleId { get; }

        public Guid ClaimId { get; }

        public string Value { get; }

        [JsonConstructor]
        internal OrganizationRoleClaim(
            Guid roleId,
            Guid claimId,
            string value
        )
        {
            RoleId = roleId;
            ClaimId = claimId;
            Value = value;
        }
    }
}