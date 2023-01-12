using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Infrastructure.Db.Repositories;

public interface IUserLoginRepository
{
    Task<UserLogin> CreateAsync(UserLogin login);
    Task<UserLogin?> TryGetByIdAsync(Guid id);
    Task<UserLogin> GetByIdAsync(Guid id);
    Task<UserLogin?> TryFindByRefreshTokenAsync(Guid token);
    Task<UserLogin> FindByRefreshTokenAsync(Guid token);
    Task<UserLogin> UpdateRefreshTokenAsync(UserLogin login);
    Task DeleteByIdAsync(Guid id);
    Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant);
    Task DeleteAllByUserIdAsync(Guid userId);
}