using System;
using System.Collections.Generic;

namespace Server.Db.Entities;

internal class Role : BaseIdEntity
{
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<RoleClaim> Claims { get; set; } = null!;
}