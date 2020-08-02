using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class UserClaim : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }
}