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

    internal class UpdateTokensCommandValidator : Validator<UpdateTokensCommand>
    {
        public UpdateTokensCommandValidator()
        {
            Field(e => e.RefreshToken).Required();
        }
    }

    internal class UpdateTokensCommandComposer : Composer<UpdateTokensCommand>
    {
        public UpdateTokensCommandComposer(
            IUserLoginRepository userLoginRepository
        )
        {
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}