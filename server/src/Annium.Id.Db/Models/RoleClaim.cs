using System;

namespace Annium.Id.Db
{
    public class RoleClaim
    {
        public Guid RoleId { get; }

        public Guid ClaimId { get; }

        public string Value { get; }

        internal RoleClaim(
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