using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Application.Commands.Login
{
    public class LogOutAppCommand : ICommand
    {
        public Guid AppId { get; }
        public Guid LoginId { get; private set; }

        public LogOutAppCommand(Guid appId)
        {
            AppId = appId;
        }
    }

    internal class LogOutAppCommandComposer : Composer<LogOutAppCommand>
    {
        public LogOutAppCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(c => c.LoginId).LoadWith(ctx => tokenAccessor.GetBaseToken().LoginId);
        }
    }
}