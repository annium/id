using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Me
{
    public class UpdateMeCommand : ICommand
    {
        public string Login { get; }
        public string Password { get; }
        public string Email { get; }
        public User User { get; private set; }

        public UpdateMeCommand(
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

    internal class UpdateMeCommandValidator : Validator<UpdateMeCommand>
    {
        public UpdateMeCommandValidator()
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Password).Required().Length(8, 50);
            Field(e => e.Email).Required().Length(3, 100).Email();
        }
    }

    internal class UpdateMeCommandComposer : Composer<UpdateMeCommand>
    {
        public UpdateMeCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
        }
    }
}