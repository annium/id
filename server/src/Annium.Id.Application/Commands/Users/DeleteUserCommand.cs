using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Users
{
    public class DeleteUserCommand : ICommand
    {
        public Guid MyId { get; private set; }

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
            Field(e => e.MyId).LoadWith(ctx => tokenAccessor.GetBaseToken().UserId);
        }
    }
}