using System;
using MessagePack;

namespace Annium.Id.Core
{
    [MessagePackObject]
    public class IdBaseToken
    {
        [Key(0)]
        public Guid UserId { get; }

        [Key(1)]
        public Guid LoginId { get; }

        public IdBaseToken(
            Guid userId,
            Guid loginId
        )
        {
            UserId = userId;
            LoginId = loginId;
        }
    }
}