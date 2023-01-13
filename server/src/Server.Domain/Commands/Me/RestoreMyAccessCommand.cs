using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Me;

public class RestoreMyAccessCommand : ICommand
{
    public Guid AppId { get; set; }
    public string Server { get; }
    public string Email { get; }
    public App App { get; private set; } = null!;
    public Uri ServerUri { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public RestoreMyAccessCommand(
        Guid appId,
        string server,
        string email
    )
    {
        AppId = appId;
        Server = server;
        Email = email;
    }
}