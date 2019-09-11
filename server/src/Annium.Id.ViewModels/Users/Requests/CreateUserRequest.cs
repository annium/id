using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class CreateUserRequest : IRequest<CreateUserCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}