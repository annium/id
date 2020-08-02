using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Validation;
using Annium.Extensions.Composition;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me
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

    internal class UpdateMyProfileCommandValidator : Validator<UpdateMyProfileCommand>
    {
        public UpdateMyProfileCommandValidator(
        )
        {
            Field(e => e.Login).Required().Length(3, 50);
            Field(e => e.Email).Required().Length(3, 100).Email();
        }
    }

    internal class UpdateMyProfileCommandComposer : Composer<UpdateMyProfileCommand>
    {
        public UpdateMyProfileCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
        }
    }
}