using System;

namespace Annium.Id.Db
{
    public class UserOrganization
    {
        public Guid UserId { get; }

        public Guid OrganizationId { get; }

        internal UserOrganization(
            Guid userId,
            Guid organizationId
        )
        {
            UserId = userId;
            OrganizationId = organizationId;
        }
    }
}