using Annium.Extensions.Composition;
using Annium.Id.Api.Domain.Commands.Me;
using Annium.Id.Core;

namespace Annium.Id.Api.Application.Commands.Me
{
    internal class UnregisterMeCommandComposer : Composer<UnregisterMeCommand>
    {
        public UnregisterMeCommandComposer(
            ITokenAccessor tokenAccessor
        )
        {
            Field(e => e.MyId).LoadWith(_ => tokenAccessor.GetToken().UserId);
        }
    }
}