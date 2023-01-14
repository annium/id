using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyRoleClaimRepository
{
    Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim);
    Task DeleteByIdAsync(Guid roleId, Guid claimId);
}