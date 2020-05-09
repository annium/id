using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyUserRepository
    {
        Task<CompanyUser> SaveAsync(CompanyUser companyUser);
        Task<CompanyUser> GetByIdAsync(Guid companyId, Guid userId);
        Task<User[]> GetAllAsync(Guid companyId);
        Task DeleteByIdAsync(Guid companyId, Guid userId);
    }
}