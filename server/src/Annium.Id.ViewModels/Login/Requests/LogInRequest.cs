using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogInRequest : LogInRequestBase, IRequest<LogInCommand>
    {
        public string AppKey { get; set; } = string.Empty;
    }

    public class LogInRequestBase
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}