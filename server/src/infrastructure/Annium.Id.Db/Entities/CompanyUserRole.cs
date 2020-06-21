using System;

namespace Annium.Id.Db.Entities
{
    internal class CompanyUserRole : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public CompanyRole Role { get; set; } = null!;
    }
}