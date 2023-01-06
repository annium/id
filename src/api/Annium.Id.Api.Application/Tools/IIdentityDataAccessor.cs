using Annium.Id.Domain.Entities.Utility;

namespace Annium.Id.Api.Application.Tools;

public interface IIdentityDataAccessor
{
    IdentityData GetIdentityData();
}