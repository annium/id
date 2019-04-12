using System.Net;

namespace Annium.Id.AspNetCore.Tools
{
    public interface IIdentityDataAccessor
    {
        (IPAddress ipAddress, string client) GetIdentityData();
    }
}