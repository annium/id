using System;
using Annium.Architecture.ViewModel;
using Annium.Id.Api.Application.Commands.Login;

namespace Annium.Id.Api.ViewModels.Requests.Login
{
    public class LogInRequest : LogInRequestBody, IRequest<LogInCommand>
    {
        public Guid AppId { get; set; }
    }

    public class LogInRequestBody
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}