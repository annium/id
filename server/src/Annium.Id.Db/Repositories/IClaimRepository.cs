using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
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