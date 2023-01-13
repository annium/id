using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Db.Repositories;

public interface ICompanyRoleClaimRepository
{
    Task<CompanyRoleClaim> SaveAsync(CompanyRoleClaim claim);
    Task DeleteByIdAsync(Guid roleId, Guid claimId);
}