using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogOutRequest : IRequest<LogOutCommand>
    {
        public string AppKey { get; set; } = string.Empty;
    }
}