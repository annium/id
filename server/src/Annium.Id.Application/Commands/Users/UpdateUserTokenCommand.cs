using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class UpdateUserTokenCommand : ICommand
    {
        public Guid RefreshToken { get; set; }
        public UserLogin Login { get; private set; }

        public UpdateUserTokenCommand(
            Guid refreshToken
        )
        {
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateUserTokenCommandValidator : Validator<UpdateUserTokenCommand>
    {
        public UpdateUserTokenCommandValidator()
        {
            Field(e => e.RefreshToken).NotEqual(Guid.Empty);
        }
    }

    internal class UpdateUserTokenCommandComposer : Composer<UpdateUserTokenCommand>
    {
        public UpdateUserTokenCommandComposer(
            IUserLoginRepository userLoginRepository
        )
        {
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}