using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Domain.Commands.Login;

namespace Annium.Id.Api.ViewModels.Requests.Login;

public class LogOutRequest : IRequest<LogOutCommand>
{
    public Guid AppId { get; set; }
}