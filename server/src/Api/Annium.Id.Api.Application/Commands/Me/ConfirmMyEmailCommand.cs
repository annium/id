using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Extensions.Validation;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.Commands.Me
{
    public class ConfirmMyEmailCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid Id { get; }
        public App App { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public ConfirmMyEmailCommand(
            Guid appId,
            Guid id
        )
        {
            AppId = appId;
            Id = id;
        }
    }

    internal class ConfirmMyEmailCommandValidator : Validator<ConfirmMyEmailCommand>
    {
        public ConfirmMyEmailCommandValidator(
        )
        {
            Field(c => c.AppId).Required();
            Field(c => c.Id).Required();
        }
    }

    internal class ConfirmMyEmailCommandComposer : Composer<ConfirmMyEmailCommand>
    {
        public ConfirmMyEmailCommandComposer(
            IAppRepository appRepository,
            IUserRepository userRepository
        )
        {
            Field(c => c.App).LoadWith(ctx => appRepository.GetByIdAsync(ctx.Root.AppId));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.Id));
        }
    }
}