using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public class CompanyRole
{
    public Guid Id { get; }
    public Guid AppId { get; }
    public string Key { get; set; }
    public string Name { get; set; }
    public IEnumerable<ClaimValue> Claims { get; }

    public CompanyRole(
        Guid appId,
        string key,
        string name,
        IEnumerable<ClaimValue> claims
    )
    {
        AppId = appId;
        Key = key;
        Name = name;
        Claims = claims;
    }

    internal CompanyRole(
        Guid id,
        Guid appId,
        string key,
        string name,
        IEnumerable<ClaimValue> claims
    ) : this(appId, key, name, claims)
    {
        Id = id;
    }
}