using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class UpdateAppTokenCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RefreshToken { get; }
        public App App { get; private set; }
        public UserAppLogin Login { get; private set; }

        public UpdateAppTokenCommand(
            Guid appId,
            Guid refreshToken
        )
        {
            AppId = appId;
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateAppTokenCommandValidator : Validator<UpdateAppTokenCommand>
    {
        public UpdateAppTokenCommandValidator()
        {
            Field(e => e.AppId).Required();
            Field(e => e.RefreshToken).Required();
        }
    }

    internal class UpdateAppTokenCommandComposer : Composer<UpdateAppTokenCommand>
    {
        public UpdateAppTokenCommandComposer(
            IAppRepository appRepository,
            IUserAppLoginRepository userAppLoginRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(e => e.Login).LoadWith(ctx => userAppLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}