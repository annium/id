using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Login
{
    public class LogOutCommand : ICommand
    {
        public Guid LoginId { get; private set; }
    }

    internal class LogOutCommandComposer : Composer<LogOutCommand>
    {
        public LogOutCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.LoginId).LoadWith(ctx => tokenAccessor.GetBaseToken().LoginId);
        }
    }
}