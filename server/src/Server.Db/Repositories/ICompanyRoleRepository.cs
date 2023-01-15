using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyRoleRepository
{
    Task CreateAsync(CompanyRole role);
    Task<IReadOnlyCollection<CompanyRole>> GetAllAsync(Guid appId);
    Task<CompanyRole?> TryGetByIdAsync(Guid id);
    Task<CompanyRole> GetByIdAsync(Guid id);
    Task<CompanyRole?> TryFindByKeyAsync(Guid appId, string key);
    Task UpdateAsync(CompanyRole role);
    Task DeleteByIdAsync(Guid id);
}