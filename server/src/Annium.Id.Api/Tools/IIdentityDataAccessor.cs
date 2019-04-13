using System.Net;

namespace Annium.Id.Api.Tools
{
    public interface IIdentityDataAccessor
    {
        (IPAddress ipAddress, string client) GetIdentityData();
    }
}