using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Login;

namespace Annium.Id.Api.ViewModels.Login.Requests
{
    public class LogInRequest : LogInRequestBase, IRequest<LogInCommand>
    {
        public Guid AppId { get; set; }
    }

    public class LogInRequestBase
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}