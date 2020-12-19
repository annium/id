using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Me
{
    public class UpdateMyProfileCommand : ICommand
    {
        public string Login { get; }
        public string Email { get; }
        public User User { get; private set; } = null!;

        public UpdateMyProfileCommand(
            string login,
            string email
        )
        {
            Login = login;
            Email = email;
        }
    }
}