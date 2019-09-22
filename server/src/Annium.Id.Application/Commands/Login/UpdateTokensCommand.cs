using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class UpdateTokensCommand : ICommand
    {
        public Guid RefreshToken { get; set; }
        public UserLogin Login { get; private set; }

        public UpdateTokensCommand(
            Guid refreshToken
        )
        {
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateUserTokenCommandValidator : Validator<UpdateTokensCommand>
    {
        public UpdateUserTokenCommandValidator()
        {
            Field(e => e.RefreshToken).Required();
        }
    }

    internal class UpdateUserTokenCommandComposer : Composer<UpdateTokensCommand>
    {
        public UpdateUserTokenCommandComposer(
            IUserLoginRepository userLoginRepository
        )
        {
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}