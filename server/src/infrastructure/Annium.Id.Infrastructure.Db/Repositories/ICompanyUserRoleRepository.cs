using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories
{
    public interface ICompanyUserRoleRepository
    {
        Task<CompanyUserRole> SaveAsync(CompanyUserRole userRole);
        Task<IReadOnlyDictionary<Guid, CompanyRole[]>> GetCompaniesUserRolesAsync(Guid appId, Guid userId);
        Task DeleteByIdAsync(Guid companyId, Guid userId, Guid roleId);
    }
}