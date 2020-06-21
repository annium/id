using System;
using Annium.Architecture.CQRS.Commands;
using Annium.Extensions.Composition;
using Annium.Id.Core;

namespace Annium.Id.Api.Application.Commands.Me
{
    public class UnregisterMeCommand : ICommand
    {
        public Guid MyId { get; private set; }

        public UnregisterMeCommand()
        {

        }
    }

    internal class UnregisterMeCommandComposer : Composer<UnregisterMeCommand>
    {
        public UnregisterMeCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(e => e.MyId).LoadWith(ctx => tokenAccessor.GetToken().UserId);
        }
    }
}