using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Login;

namespace Annium.Id.ViewModels.Login.Requests
{
    public class LogUserInRequest : IRequest<LogUserInCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}