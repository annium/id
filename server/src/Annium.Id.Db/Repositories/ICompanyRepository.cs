using System;
using System.Threading.Tasks;

namespace Annium.Id.Db.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> CreateAsync(Company company);

        Task<Company[]> GetAllAsync();

        Task<Company> GetByIdAsync(Guid id);

        Task<Company> FindByKeyAsync(string key);

        Task<Company> UpdateAsync(Company company);

        Task DeleteByIdAsync(Guid id);
    }
}