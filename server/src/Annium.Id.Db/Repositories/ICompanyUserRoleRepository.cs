using System;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyUserRoleRepository
    {
        Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole);

        Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId);
    }
}