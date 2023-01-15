using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IClaimRepository
{
    Task CreateAsync(Claim claim);
    Task<IReadOnlyCollection<Claim>> GetAllAsync(Guid appId);
    Task<Claim?> TryGetByIdAsync(Guid id);
    Task<Claim> GetByIdAsync(Guid id);
    Task<Claim?> TryFindByKeyAsync(Guid appId, string key);
    Task UpdateAsync(Claim claim);
    Task DeleteByIdAsync(Guid id);
}