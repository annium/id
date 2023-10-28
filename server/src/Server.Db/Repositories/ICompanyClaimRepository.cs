using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyClaimRepository
{
    Task CreateAsync(CompanyClaim companyClaim);
    Task<IReadOnlyCollection<CompanyClaim>> GetAllAsync(Guid appId);
    Task<CompanyClaim?> TryGetByIdAsync(Guid id);
    Task<CompanyClaim> GetByIdAsync(Guid id);
    Task<CompanyClaim?> TryFindByKeyAsync(Guid appId, string key);
    Task UpdateAsync(CompanyClaim companyClaim);
    Task DeleteByIdAsync(Guid id);
}
