using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Users
{
    public class LogUserOutAppCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid LoginId { get; private set; }

        public LogUserOutAppCommand(Guid appId)
        {
            AppId = appId;
        }
    }

    internal class LogUserOutAppCommandComposer : Composer<LogUserOutAppCommand>
    {
        public LogUserOutAppCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.LoginId).LoadWith(ctx => tokenAccessor.GetBaseToken().LoginId);
        }
    }
}