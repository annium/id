using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Login;

public class LogInCommand : ICommand
{
    public Guid AppId { get; }
    public string Login { get; }
    public string Password { get; }
    public App App { get; private set; } = null!;
    public User User { get; private set; } = null!;

    public LogInCommand(
        Guid appId,
        string login,
        string password
    )
    {
        AppId = appId;
        Login = login;
        Password = password;
    }
}