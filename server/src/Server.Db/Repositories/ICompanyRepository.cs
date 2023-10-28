using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface ICompanyRepository
{
    Task CreateAsync(Company company);
    Task<IReadOnlyCollection<Company>> FindAllAsync(string name);
    Task<IReadOnlyCollection<Company>> FindMyAsync(Guid ownerId);
    Task<IReadOnlyCollection<Company>> GetAllByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task<Company?> TryGetByIdAsync(Guid id);
    Task<Company> GetByIdAsync(Guid id);
    Task UpdateAsync(Company company);
    Task DeleteByIdAsync(Guid id);
}
