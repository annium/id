using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IOrganizationClaimRepository
    {
        Task<OrganizationClaim> CreateAsync(OrganizationClaim OrganizationClaim);

        Task<OrganizationClaim[]> GetAllAsync(Guid appId);

        Task<OrganizationClaim> GetByIdAsync(Guid id);

        Task<OrganizationClaim> FindByKeyAsync(Guid appId, string key);

        Task<OrganizationClaim> UpdateAsync(OrganizationClaim OrganizationClaim);

        Task DeleteByIdAsync(Guid id);
    }
}