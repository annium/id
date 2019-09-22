using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Login
{
    public class LogUserOutCommand : ICommand
    {
        public Guid LoginId { get; private set; }
    }

    internal class LogUserOutCommandComposer : Composer<LogUserOutCommand>
    {
        public LogUserOutCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.LoginId).LoadWith(ctx => tokenAccessor.GetBaseToken().LoginId);
        }
    }
}