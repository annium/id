using System;
using Annium.Architecture.ViewModel;
using Server.Domain.Commands.Login;

namespace Server.ViewModels.Requests.Login;

public record LogOutRequest : IRequest<LogOutCommand>
{
    public Guid AppId { get; set; }
}