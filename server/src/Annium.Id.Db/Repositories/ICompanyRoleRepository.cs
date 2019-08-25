using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyRoleRepository
    {
        Task<CompanyRole> CreateAsync(CompanyRole role);

        Task<CompanyRole[]> GetAllAsync(Guid appId);

        Task<CompanyRole> GetByIdAsync(Guid id);

        Task<CompanyRole> FindByKeyAsync(Guid appId, string key);

        Task<CompanyRole> UpdateAsync(CompanyRole role);

        Task DeleteByIdAsync(Guid id);
    }
}