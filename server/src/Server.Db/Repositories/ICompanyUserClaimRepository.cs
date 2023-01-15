using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyUserClaimRepository
{
    Task SaveAsync(CompanyUserClaim claim);
    Task<IReadOnlyDictionary<Guid, IReadOnlyCollection<CompanyUserClaim>>> GetCompaniesUserClaimsAsync(Guid appId, Guid userId);
    Task DeleteByIdAsync(Guid companyId, Guid userId, Guid claimId);
}