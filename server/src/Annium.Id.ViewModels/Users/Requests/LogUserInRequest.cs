using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class LogUserInRequest : IRequest<LogUserInCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}