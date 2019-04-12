using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class UserClaim
    {
        public Guid UserId { get; }

        public Guid ClaimId { get; }

        public string Value { get; }

        [JsonConstructor]
        internal UserClaim(
            Guid userId,
            Guid claimId,
            string value
        )
        {
            UserId = userId;
            ClaimId = claimId;
            Value = value;
        }
    }
}