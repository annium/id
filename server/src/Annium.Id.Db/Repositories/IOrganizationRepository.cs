using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateAsync(Organization organization);

        Task<Organization[]> GetAllAsync();

        Task<Organization> GetByIdAsync(Guid id);

        Task<Organization> FindByKeyAsync(string key);

        Task<Organization> UpdateAsync(Organization organization);

        Task DeleteByIdAsync(Guid id);
    }
}