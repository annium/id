using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Db.Repositories;

public interface IClaimRepository
{
    Task<Claim> CreateAsync(Claim claim);
    Task<Claim[]> GetAllAsync(Guid appId);
    Task<Claim?> TryGetByIdAsync(Guid id);
    Task<Claim> GetByIdAsync(Guid id);
    Task<Claim?> TryFindByKeyAsync(Guid appId, string key);
    Task<Claim> UpdateAsync(Claim claim);
    Task DeleteByIdAsync(Guid id);
}