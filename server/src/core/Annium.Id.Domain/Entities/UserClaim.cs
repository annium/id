using System;

namespace Annium.Id.Domain.Entities
{
    public class UserClaim
    {
        public Guid UserId { get; }
        public Guid ClaimId { get; }
        public string Value { get; set; }

        public UserClaim(
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