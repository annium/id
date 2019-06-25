using System;

namespace Annium.Id.Db
{
    public class UserRole
    {
        public Guid UserId { get; }
        public Guid RoleId { get; }

        public UserRole(
            Guid userId,
            Guid roleId
        )
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}