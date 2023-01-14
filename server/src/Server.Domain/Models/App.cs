using System;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class App : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; } = default!;
    public string Name { get; private set; } = string.Empty;
    public Guid ApiToken { get; private set; }

    public App(
        User owner,
        string name,
        Guid apiToken
    )
    {
        Id = Guid.NewGuid();
        OwnerId = owner.Id;
        Owner = owner;
        Name = name;
        ApiToken = apiToken;
    }

    internal App()
    {
    }

    public void SetName(string name) => Name = name;
    public void SetApiToken(Guid apiToken) => ApiToken = apiToken;

    public void SetOwner(User owner)
    {
        OwnerId = owner.Id;
        Owner = owner;
    }
}