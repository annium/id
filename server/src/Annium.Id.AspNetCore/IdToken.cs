using System;
using MessagePack;

namespace Annium.Id.AspNetCore
{
    [MessagePackObject]
    public class IdToken
    {
        [Key(0)]
        public Guid UserId { get; }

        public IdToken(
            Guid userId
        )
        {
            UserId = userId;
        }
    }
}