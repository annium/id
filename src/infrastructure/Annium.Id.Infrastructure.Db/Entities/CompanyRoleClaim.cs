using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class CompanyRoleClaim : BaseEntity
    {
        public Guid RoleId { get; set; }
        public CompanyRole Role { get; set; } = null!;
        public Guid ClaimId { get; set; }
        public CompanyClaim Claim { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }
}