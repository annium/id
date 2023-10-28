using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyUserRepository
{
    Task SaveAsync(CompanyUser companyUser);
    Task<CompanyUser?> TryGetByIdAsync(Guid companyId, Guid userId);
    Task<IReadOnlyCollection<User>> GetAllAsync(Guid companyId);
    Task DeleteByIdAsync(Guid companyId, Guid userId);
}
