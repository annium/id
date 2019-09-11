using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Users
{
    public class UpdateUserAppTokenCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid RefreshToken { get; }
        public App App { get; private set; }
        public UserAppLogin Login { get; private set; }

        public UpdateUserAppTokenCommand(
            Guid appId,
            Guid refreshToken
        )
        {
            AppId = appId;
            RefreshToken = refreshToken;
        }
    }

    internal class UpdateUserAppTokenCommandValidator : Validator<UpdateUserAppTokenCommand>
    {
        public UpdateUserAppTokenCommandValidator()
        {
            Field(e => e.AppId).NotEqual(Guid.Empty);
            Field(e => e.RefreshToken).NotEqual(Guid.Empty);
        }
    }

    internal class UpdateUserAppTokenCommandComposer : Composer<UpdateUserAppTokenCommand>
    {
        public UpdateUserAppTokenCommandComposer(
            IAppRepository appRepository,
            IUserAppLoginRepository userAppLoginRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(e => e.Login).LoadWith(ctx => userAppLoginRepository.FindByRefreshTokenAsync(ctx.Root.RefreshToken));
        }
    }
}