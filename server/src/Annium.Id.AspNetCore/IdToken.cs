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
        public Guid LoginId { get; }

        public IdToken(
            Guid userId,
            Guid loginId
        )
        {
            UserId = userId;
            LoginId = loginId;
        }
    }
}