using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class RoleClaim : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Guid ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }
}