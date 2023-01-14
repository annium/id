using System;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyUserRepository
{
    Task<CompanyUser> SaveAsync(CompanyUser companyUser);
    Task<CompanyUser?> TryGetByIdAsync(Guid companyId, Guid userId);
    Task<User[]> GetAllAsync(Guid companyId);
    Task DeleteByIdAsync(Guid companyId, Guid userId);
}