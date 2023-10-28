using System.Net;

namespace Server.Application.Tools;

public class IdentityData
{
    public IPAddress IpAddress { get; }
    public string Client { get; }

    public IdentityData(IPAddress ipAddress, string client)
    {
        IpAddress = ipAddress;
        Client = client;
    }
}
