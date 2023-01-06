using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Claims;

public class CreateClaimCommand : ICommand
{
    public Guid AppId { get; }
    public string Key { get; }
    public string Name { get; }
    public Guid MyId { get; private set; }
    public App App { get; private set; } = null!;

    public CreateClaimCommand(
        Guid appId,
        string key,
        string name
    )
    {
        AppId = appId;
        Key = key;
        Name = name;
    }
}