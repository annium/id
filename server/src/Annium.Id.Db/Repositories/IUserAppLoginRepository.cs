using System;
using System.Threading.Tasks;
using Annium.Id.Domain.Entities;
using NodaTime;

namespace Annium.Id.Db.Repositories
{
    public interface IUserAppLoginRepository
    {
        Task<UserAppLogin> CreateAsync(UserAppLogin login);

        Task<UserAppLogin> FindByRefreshTokenAsync(Guid token);

        Task<UserAppLogin> UpdateRefreshTokenAsync(UserAppLogin login);

        Task DeleteByIdAsync(Guid id);

        Task DeleteExpiredByUserIdAsync(Guid userId, Instant instant);

        Task DeleteAllByUserIdAsync(Guid userId);
    }
}