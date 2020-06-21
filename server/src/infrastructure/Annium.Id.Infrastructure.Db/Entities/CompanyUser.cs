using System;

namespace Annium.Id.Infrastructure.Db.Entities
{
    internal class CompanyUser : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}