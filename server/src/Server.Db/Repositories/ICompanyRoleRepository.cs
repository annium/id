using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Db.Repositories;

public interface ICompanyRoleRepository
{
    Task<CompanyRole> CreateAsync(CompanyRole role);
    Task<CompanyRole[]> GetAllAsync(Guid appId);
    Task<CompanyRole?> TryGetByIdAsync(Guid id);
    Task<CompanyRole> GetByIdAsync(Guid id);
    Task<CompanyRole?> TryFindByKeyAsync(Guid appId, string key);
    Task<CompanyRole> UpdateAsync(CompanyRole role);
    Task DeleteByIdAsync(Guid id);
}