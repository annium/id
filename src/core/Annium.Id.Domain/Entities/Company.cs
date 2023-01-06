using System;

namespace Annium.Id.Domain.Entities;

public class Company
{
    public Guid Id { get; }
    public Guid OwnerId { get; set; }
    public Guid? ParentId { get; set; }
    public string Name { get; set; }

    public Company(
        Guid ownerId,
        Guid? parentId,
        string name
    )
    {
        OwnerId = ownerId;
        ParentId = parentId;
        Name = name;
    }

    internal Company(
        Guid id,
        Guid ownerId,
        Guid? parentId,
        string name
    ) : this(ownerId, parentId, name)
    {
        Id = id;
    }
}