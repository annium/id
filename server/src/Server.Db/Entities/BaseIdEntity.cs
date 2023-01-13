using System;

namespace Server.Db.Entities;

internal abstract class BaseIdEntity : BaseEntity
{
    public Guid Id { get; set; }
}