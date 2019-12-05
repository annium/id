using Annium.Id.Domain.Entities.Utility;

namespace Annium.Id.Application.Tools
{
    public interface IIdentityDataAccessor
    {
        IdentityData GetIdentityData();
    }
}