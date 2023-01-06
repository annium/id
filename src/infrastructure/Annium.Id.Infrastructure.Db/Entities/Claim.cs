using System;

namespace Annium.Id.Infrastructure.Db.Entities;

internal class Claim : BaseIdEntity
{
    public Guid AppId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}