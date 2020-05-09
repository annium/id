using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Login;

namespace Annium.Id.Api.ViewModels.Login.Requests
{
    public class LogOutRequest : IRequest<LogOutCommand>
    {
        public Guid AppId { get; set; }
    }
}