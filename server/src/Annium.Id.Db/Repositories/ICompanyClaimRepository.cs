using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyClaimRepository
    {
        Task<CompanyClaim> CreateAsync(CompanyClaim CompanyClaim);
        Task<CompanyClaim[]> GetAllAsync(Guid appId);
        Task<CompanyClaim> GetByIdAsync(Guid id);
        Task<CompanyClaim> FindByKeyAsync(Guid appId, string key);
        Task<CompanyClaim> UpdateAsync(CompanyClaim CompanyClaim);
        Task DeleteByIdAsync(Guid id);
    }
}