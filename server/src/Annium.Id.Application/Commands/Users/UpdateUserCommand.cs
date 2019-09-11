using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class UpdateUserCommand : ICommand
    {
        public string Login { get; }
        public string Password { get; }
        public string Email { get; }
        public User User { get; private set; }

        public UpdateUserCommand(
            string login,
            string password,
            string email
        )
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }

    internal class UpdateUserCommandValidator : Validator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email();
        }
    }

    internal class UpdateUserCommandComposer : Composer<UpdateUserCommand>
    {
        public UpdateUserCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetBaseToken().UserId));
        }
    }
}