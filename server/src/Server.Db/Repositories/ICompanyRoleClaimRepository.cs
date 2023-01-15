using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyRoleClaimRepository
{
    Task SaveAsync(CompanyRoleClaim claim);
    Task DeleteByIdAsync(Guid roleId, Guid claimId);
}