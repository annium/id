using System;

namespace Annium.Id.Infrastructure.Db.Entities;

internal class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}