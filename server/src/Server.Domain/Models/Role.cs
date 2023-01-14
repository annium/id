using System;
using System.Collections.Generic;
using System.Linq;
using Annium.Data.Models;

namespace Server.Domain.Models;

public class Role : IIdEntity<Guid>
{
    public Guid Id { get; private init; }
    public Guid AppId { get; private init; }
    public App App { get; private init; } = default!;
    public string Key { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<RoleClaim> Claims
    {
        get => _claims;
        private init => _claims = value.ToList();
    }

    private readonly List<RoleClaim> _claims = new();

    public Role(
        App app,
        string key,
        string name
    )
    {
        Id = Guid.NewGuid();
        AppId = app.Id;
        App = app;
        Key = key;
        Name = name;
    }

    internal Role()
    {
    }

    public void Update(string key, string name)
    {
        Key = key;
        Name = name;
    }
}