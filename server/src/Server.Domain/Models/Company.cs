using System;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class Company : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; } = default!;
    public Guid? ParentId { get; private set; }
    public Company? Parent { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public Company(
        User owner,
        Company? parent,
        string name
    )
    {
        Id = Guid.NewGuid();
        OwnerId = owner.Id;
        Owner = owner;
        ParentId = parent?.Id;
        Parent = parent;
        Name = name;
    }

    internal Company()
    {
    }

    public void Update(Company? parent, string name)
    {
        ParentId = parent?.Id;
        Parent = parent;
        Name = name;
    }

    public void SetOwner(User user)
    {
        Owner = user;
    }
}