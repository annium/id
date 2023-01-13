using Annium.Extensions.Composition;
using Annium.Id.Core;
using Server.Domain.Commands.Me;

namespace Server.Application.Commands.Me;

internal class UnregisterMeCommandComposer : Composer<UnregisterMeCommand>
{
    public UnregisterMeCommandComposer(
        ITokenAccessor tokenAccessor
    )
    {
        Field(e => e.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
    }
}