using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyClaimRepository
{
    Task<CompanyClaim> CreateAsync(CompanyClaim companyClaim);
    Task<CompanyClaim[]> GetAllAsync(Guid appId);
    Task<CompanyClaim?> TryGetByIdAsync(Guid id);
    Task<CompanyClaim> GetByIdAsync(Guid id);
    Task<CompanyClaim?> TryFindByKeyAsync(Guid appId, string key);
    Task<CompanyClaim> UpdateAsync(CompanyClaim companyClaim);
    Task DeleteByIdAsync(Guid id);
}