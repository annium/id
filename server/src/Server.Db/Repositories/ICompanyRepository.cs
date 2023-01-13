using System;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Server.Db.Repositories;

public interface ICompanyRepository
{
    Task<Company> CreateAsync(Company company);
    Task<Company[]> FindAllAsync(string name);
    Task<Company[]> FindMyAsync(Guid ownerId);
    Task<Company[]> GetAllByIdsAsync(Guid[] ids);
    Task<Company?> TryGetByIdAsync(Guid id);
    Task<Company> GetByIdAsync(Guid id);
    Task<Company> UpdateAsync(Company company);
    Task DeleteByIdAsync(Guid id);
}