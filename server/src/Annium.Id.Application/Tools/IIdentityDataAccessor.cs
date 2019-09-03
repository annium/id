using System.Net;

namespace Annium.Id.Application.Tools
{
    public interface IIdentityDataAccessor
    {
        (IPAddress ipAddress, string client) GetIdentityData();
    }
}