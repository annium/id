using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.User.Requests
{
    public class LoginUserRequest : IRequest<LoginUserCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}