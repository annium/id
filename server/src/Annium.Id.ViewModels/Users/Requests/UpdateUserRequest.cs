using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands.Users;

namespace Annium.Id.ViewModels.Users.Requests
{
    public class UpdateUserRequest : IRequest<UpdateUserCommand>
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}