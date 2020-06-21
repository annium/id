using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Domain.Entities;
using Annium.Id.Infrastructure.Db.Repositories;

namespace Annium.Id.Api.Application.Commands.Login
{
    public class LogOutCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid LoginId { get; private set; }
        public App App { get; private set; } = null!;

        public LogOutCommand(
            Guid appId
        )
        {
            AppId = appId;
        }
    }

    internal class LogOutCommandValidator : Validator<LogOutCommand>
    {
        public LogOutCommandValidator()
        {
            Field(e => e.AppId).Required();
        }
    }

    internal class LogOutCommandComposer : Composer<LogOutCommand>
    {
        public LogOutCommandComposer(
            ITokenAccessor tokenAccessor,
            IAppRepository appRepository
        )
        {
            Field(c => c.LoginId).LoadWith(ctx => tokenAccessor.GetToken().LoginId);
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
        }
    }
}