using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Domain.Models;

namespace Server.Db.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user);
    Task<User?> TryGetByIdAsync(Guid id);
    Task<User> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<User>> FindAllByQueryAsync(string query, int limit);
    Task<User?> TryFindByLoginAsync(string login);
    Task<User> FindByLoginAsync(string login);
    Task<User?> TryFindByEmailAsync(string email);
    Task<User> FindByEmailAsync(string email);
    Task UpdateAsync(User user);
    Task DeleteByIdAsync(Guid id);
}
