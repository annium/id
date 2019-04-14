using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IOrganizationRoleRepository
    {
        Task< OrganizationRole> CreateAsync( OrganizationRole role);

        Task< OrganizationRole[]> GetAllAsync(Guid appId);

        Task< OrganizationRole> GetByIdAsync(Guid id);

        Task< OrganizationRole> FindByKeyAsync(Guid appId, string key);

        Task< OrganizationRole> UpdateAsync( OrganizationRole role);

        Task DeleteByIdAsync(Guid id);
    }
}