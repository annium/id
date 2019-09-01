using Annium.Architecture.ViewModel;
using Annium.Id.Application.Commands;

namespace Annium.Id.ViewModels.User.Requests
{
    public class CreateUpdateUserRequest : IRequest<CreateUserCommand>
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}