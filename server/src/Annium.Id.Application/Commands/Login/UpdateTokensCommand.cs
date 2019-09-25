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
        public string AppKey { get; }
        public Guid RefreshToken { get; }
        public App App { get; private set; }
        public UserLogin Login { get; private set; }

        public UpdateTokensCommand(
            string appKey,
            Guid refreshToken
        )
        {
            AppKey = appKey;
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateTokensCommandValidator : Validator<UpdateTokensCommand>
    {
        public UpdateTokensCommandValidator()
        {
            Field(e => e.AppKey).Required().Length(2, 100);
            Field(e => e.RefreshToken).Required();
        }
    }

    internal class UpdateTokensCommandComposer : Composer<UpdateTokensCommand>
    {
        public UpdateTokensCommandComposer(
            IAppRepository appRepository,
            IUserLoginRepository userLoginRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.FindByKeyAsync(ctx.Root.AppKey));
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}