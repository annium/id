using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IOrganizationRoleClaimRepository
    {
        Task<OrganizationRoleClaim> SaveAsync(OrganizationRoleClaim claim);

        Task DeleteByIdAsync(Guid roleId, Guid claimId);
    }
}