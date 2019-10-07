using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Core;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Login
{
    public class LogOutCommand : ICommand
    {
        public string AppKey { get; }
        public Guid LoginId { get; private set; }
        public App App { get; private set; } = null!;

        public LogOutCommand(string appKey)
        {
            AppKey = appKey;
        }
    }

    internal class LogOutCommandValidator : Validator<LogOutCommand>
    {
        public LogOutCommandValidator()
        {
            Field(e => e.AppKey).Required().Length(2, 100);
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
            Field(c => c.App).LoadWith(ctx => appRepository.FindByKeyAsync(ctx.Root.AppKey));
        }
    }
}