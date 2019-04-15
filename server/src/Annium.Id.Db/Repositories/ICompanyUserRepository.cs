using System;
using System.Threading.Tasks;

namespace Annium.Id.Db
{
    public interface ICompanyUserRepository
    {
        Task<CompanyUser> SaveAsync(CompanyUser companyUser);

        Task<CompanyUser> GetByIdAsync(Guid companyId, Guid userId);

        Task<User[]> GetAllAsync(Guid companyId);

        Task DeleteByIdAsync(Guid companyId, Guid userId);
    }
}