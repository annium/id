using System;
using Newtonsoft.Json;

namespace Annium.Id.Db
{
    public class UserRole
    {
        public Guid UserId { get; }

        public Guid RoleId { get; }

        [JsonConstructor]
        internal UserRole(
            Guid userId,
            Guid roleId
        )
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}