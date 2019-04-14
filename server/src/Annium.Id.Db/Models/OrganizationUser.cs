using System;

namespace Annium.Id.Db
{
    public class OrganizationUser
    {
        public Guid OrganizationId { get; }

        public Guid UserId { get; }

        public OrganizationUser(
            Guid organizationId,
            Guid userId
        )
        {
            OrganizationId = organizationId;
            UserId = userId;
        }
    }
}