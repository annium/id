using System;

namespace Annium.Id.Db
{
    public class OrganizationUserRole
    {
        public Guid OrganizationId { get; }

        public Guid UserId { get; }

        public Guid RoleId { get; }

        public OrganizationUserRole(
            Guid organizationId,
            Guid userId,
            Guid roleId
        )
        {
            OrganizationId = organizationId;
            UserId = userId;
            RoleId = roleId;
        }
    }
}