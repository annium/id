using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Application.Commands.Login
{
    public class UpdateTokensCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RefreshToken { get; }
        public App App { get; private set; } = null!;
        public UserLogin Login { get; private set; } = null!;

        public UpdateTokensCommand(
            Guid appId,
            Guid refreshToken
        )
        {
            AppId = appId;
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateTokensCommandValidator : Validator<UpdateTokensCommand>
    {
        public UpdateTokensCommandValidator()
        {
            Field(e => e.AppId).Required();
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
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(e => e.Login).LoadWith(ctx => userLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}