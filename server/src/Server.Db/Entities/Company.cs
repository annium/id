using System;

namespace Server.Db.Entities;

internal class Company : BaseIdEntity
{
    public Guid OwnerId { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
}