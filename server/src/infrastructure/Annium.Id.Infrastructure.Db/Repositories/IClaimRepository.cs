using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories
{
    public interface IClaimRepository
    {
        Task<Claim> CreateAsync(Claim Claim);
        Task<Claim[]> GetAllAsync(Guid appId);
        Task<Claim> GetByIdAsync(Guid id);
        Task<Claim> FindByKeyAsync(Guid appId, string key);
        Task<Claim> UpdateAsync(Claim Claim);
        Task DeleteByIdAsync(Guid id);
    }
}