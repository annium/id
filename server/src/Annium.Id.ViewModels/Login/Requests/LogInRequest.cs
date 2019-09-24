using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogInRequest : IRequest<LogInCommand>
    {
        public string AppKey { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}