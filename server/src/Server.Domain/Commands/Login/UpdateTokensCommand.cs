using System;
using Annium.Architecture.CQRS.Commands;
using Core.Domain.Entities;

namespace Server.Domain.Commands.Login;

public class UpdateTokensCommand : ICommand
{
    public Guid AppId { get; }
    public Guid RefreshToken { get; }
    public App App { get; private set; } = null!;
    public UserLogin Login { get; private set; } = null!;

    public UpdateTokensCommand(
        Guid appId,
        Guid refreshToken
    )
    {
        AppId = appId;
        RefreshToken = refreshToken;
    }
}