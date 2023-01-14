using System;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class CompanyClaim : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid AppId { get; private init; }
    public App App { get; private init; } = default;
    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public CompanyClaim(
        Guid appId,
        string key,
        string name
    )
    {
        Id = Guid.NewGuid();
        AppId = appId;
        Key = key;
        Name = name;
    }

    internal CompanyClaim()
    {
    }

    public void Update(string key, string name)
    {
        Key = key;
        Name = name;
    }
}