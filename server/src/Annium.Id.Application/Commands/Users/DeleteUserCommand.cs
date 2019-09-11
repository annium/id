using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Users
{
    public class DeleteUserCommand : ICommand
    {
        public Guid UserId { get; private set; }

        public DeleteUserCommand()
        {

        }
    }

    internal class DeleteUserCommandComposer : Composer<DeleteUserCommand>
    {
        public DeleteUserCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(e => e.UserId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
        }
    }
}