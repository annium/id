using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Login;

namespace Server.ViewModels.Requests.Login;

public record LogInRequest : LogInRequestBody, IRequest<LogInCommand>
{
    public Guid AppId { get; set; }
}

public record LogInRequestBody
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
