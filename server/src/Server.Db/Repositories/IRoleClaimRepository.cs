using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IRoleClaimRepository
{
    Task SaveAsync(RoleClaim claim);
    Task DeleteByIdAsync(Guid roleId, Guid claimId);
}