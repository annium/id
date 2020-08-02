using System.Net;

namespace Annium.Id.Domain.Entities.Utility
{
    public class IdentityData
    {
        public IPAddress IPAddress { get; }
        public string Client { get; }

        public IdentityData(
            IPAddress ipAddress,
            string client
        )
        {
            IPAddress = ipAddress;
            Client = client;
        }
    }
}