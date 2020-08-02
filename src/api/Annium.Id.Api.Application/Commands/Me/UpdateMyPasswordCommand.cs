using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Me
{
    public class UpdateMyPasswordCommand : ICommand
    {
        public string Password { get; }
        public User User { get; private set; } = null!;

        public UpdateMyPasswordCommand(
            string password
        )
        {
            Password = password;
        }
    }

    internal class UpdateMyPasswordCommandValidator : Validator<UpdateMyPasswordCommand>
    {
        public UpdateMyPasswordCommandValidator(
        )
        {
            Field(e => e.Password).Required().Length(8, 50);
        }
    }

    internal class UpdateMyPasswordCommandComposer : Composer<UpdateMyPasswordCommand>
    {
        public UpdateMyPasswordCommandComposer(
            ITokenAccessor tokenAccessor,
            IUserRepository userRepository
        )
        {
            Field(e => e.User).LoadWith(ctx => userRepository.GetByIdAsync(tokenAccessor.GetToken().UserId));
        }
    }
}