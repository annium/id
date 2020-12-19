using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Api.Domain.Commands.Me
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
}