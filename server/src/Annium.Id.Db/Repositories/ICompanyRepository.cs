using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);

        Task<Company[]> GetAllByIdsAsync(Guid[] ids);

        Task<Company> GetByIdAsync(Guid id);

        Task<Company> FindByKeyAsync(string key);

        Task<Company> UpdateAsync(Company company);

        Task DeleteByIdAsync(Guid id);
    }
}