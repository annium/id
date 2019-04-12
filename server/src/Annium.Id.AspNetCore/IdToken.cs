using System;
using MessagePack;

namespace Annium.Id.AspNetCore
{
    [MessagePackObject]
    public class IdToken
    {
        [Key(0)]
        public Guid UserId { get; }

        [Key(1)]
        public string IPAddress { get; }

        [Key(2)]
        public string Client { get; }

        public IdToken(
            Guid userId,
            string iPAddress,
            string client
        )
        {
            UserId = userId;
            IPAddress = iPAddress;
            Client = client;
        }
    }
}