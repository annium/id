using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Infrastructure.Db.Repositories;

public interface ICompanyRepository
{
    Task<Company> CreateAsync(Company company);
    Task<Company[]> FindAllAsync(string name);
    Task<Company[]> FindMyAsync(Guid ownerId);
    Task<Company[]> GetAllByIdsAsync(Guid[] ids);
    Task<Company?> GetByIdAsync(Guid id);
    Task<Company> UpdateAsync(Company company);
    Task DeleteByIdAsync(Guid id);
}