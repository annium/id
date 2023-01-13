using System;

namespace Server.Db.Entities;

internal class CompanyUser : BaseEntity
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}