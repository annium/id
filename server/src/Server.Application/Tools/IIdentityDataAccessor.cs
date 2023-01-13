using Core.Domain.Entities.Utility;

namespace Server.Application.Tools;

public interface IIdentityDataAccessor
{
    IdentityData GetIdentityData();
}