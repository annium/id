using System;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class Claim : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid AppId { get; private init; }
    public App App { get; private init; } = default!;
    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public Claim(
        App app,
        string key,
        string name
    )
    {
        Id = Guid.NewGuid();
        AppId = app.Id;
        Key = key;
        Name = name;
    }

    internal Claim()
    {
    }

    public void Update(string key, string name)
    {
        Key = key;
        Name = name;
    }
}