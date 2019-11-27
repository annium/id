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
        public string AppKey { get; }
        public Guid Id { get; }
        public App App { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public ConfirmMyEmailCommand(
            string appKey,
            Guid id
        )
        {
            AppKey = appKey;
            Id = id;
        }
    }

    internal class ConfirmMyEmailCommandValidator : Validator<ConfirmMyEmailCommand>
    {
        public ConfirmMyEmailCommandValidator(
        )
        {
            Field(c => c.AppKey).Required();
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
            Field(c => c.App).LoadWith(ctx => appRepository.FindByKeyAsync(ctx.Root.AppKey));
            Field(c => c.User).LoadWith(ctx => userRepository.GetByIdAsync(ctx.Root.Id));
        }
    }
}